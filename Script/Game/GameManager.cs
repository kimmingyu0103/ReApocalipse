using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject[] ground;
    public GameObject[] boss;
    public GameObject shop;
    public GameObject chall;
    public GameObject over;

    [SerializeField] TMP_Text t_round;
    [SerializeField] TMP_Text t_over;

    [SerializeField] SoundManager soundManager;

    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // instance = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    public int row = 3, col = 3, MaxRow = 7, MaxCol = 7, count = 5, MaxCount = 5;
    public List<GameObject> maps = new List<GameObject>();
    public List<int> map_info = new List<int>();
    public List<GameObject> bodys = new List<GameObject>();
    public Vector3 v;
    public int mob_cnt = 0;
    public bool isMapMoving = false;
    public int round = 1, MaxRound = 3;
    public bool isGameOver = true;
    public bool mapReset = true;
    public GameObject stat;
    public float level;

    public void MobDestroyed()
    {
        if (--mob_cnt <= 0) maps[row + col * MaxRow].GetComponent<GroundManager>().ClearMap();    
    }

    private void MakeGround()
    {
        if (maps[row + col * MaxRow] == null)
        {
            maps[row + col * MaxRow] = Instantiate(ground[Random.Range(0,3)], v, Quaternion.identity); count--;
            maps[row + col * MaxRow].name = row.ToString() + "." + col.ToString();
            map_info[row + col * MaxRow] = 1; // 기본
        }
    }

    private void goTwice()
    {
        int direction = Random.Range(0, 4); // 상 하 좌 우

        switch (direction)
        {
            case 0: // up
                if (col - 2 >= 0)
                {
                    col--; v += new Vector3(0f, 14f, 0f);
                    MakeGround();
                    col--; v += new Vector3(0f, 14f, 0f);
                    MakeGround();
                }
                break;
            case 1: // down
                if (col + 2 < MaxCol)
                {
                    col++; v += new Vector3(0f, -14f, 0f);
                    MakeGround();
                    col++; v += new Vector3(0f, -14f, 0f);
                    MakeGround();
                }
                break;
            case 2: // left
                if (row - 2 >= 0)
                {
                    row--; v += new Vector3(-25f, 0f, 0f);
                    MakeGround();
                    row--; v += new Vector3(-25f, 0f, 0f);
                    MakeGround();
                }
                break;
            case 3: // right
                if (row + 2 < MaxRow)
                {
                    row++; v += new Vector3(25f, 0f, 0f);
                    MakeGround();
                    row++; v += new Vector3(25f, 0f, 0f);
                    MakeGround();
                }
                break;
        }
    }

    private void BacknForth()
    {
        int direction = Random.Range(0, 2);

        switch (direction)
        {
            case 0: // up down
                if (col - 1 >= 0 && col + 1 < MaxCol)
                {
                    col--; v += new Vector3(0f, 14f, 0f);
                    MakeGround();
                    col += 2; v += new Vector3(0f, -28f, 0f);
                    MakeGround();
                    col--; v += new Vector3(0f, 14f, 0f);
                }
                break;
            case 1: // left right
                if (row - 1 >= 0 && row + 1 < MaxRow)
                {
                    row--; v += new Vector3(-25f, 0f, 0f);
                    MakeGround();
                    row += 2; v += new Vector3(50f, 0f, 0f);
                    MakeGround();
                    row--; v += new Vector3(-25f, 0f, 0f);
                }
                break;
        }
    }

    private void MakeMap()
    {
        while(count > 0) {

            int method = Random.Range(0, 2);
            // 0 -> 좌우생성 1-> 선택방향으로 두번 이동

            if (method == 1) goTwice();
            else BacknForth();
        }
        makeBossMap();
        col = 3; row = 3;
    }

    public void Move(int direction) {
        switch (direction) {
            case 0: col--; break;
            case 1: col++; break;
            case 2: row--; break;
            case 3: row++; break;
        }
        maps[row + col * MaxRow].GetComponent<GroundManager>().GenerateMob();
        if (map_info[row + col * MaxRow] == 2 && level>=1)
        { chall.SetActive(true);  Time.timeScale = 0; soundManager.PlayBGM(2); }
    }

    private void makeBossMap()
    {
        while (maps[row + col * MaxRow] != null) {

            int direction = Random.Range(0, 4); // 상 하 좌 우

            switch (direction)
            {
                case 0: // up
                    if (col - 2 >= 0) { col--; v += new Vector3(0f, 14f, 0f); }
                    break;
                case 1: // down
                    if (col + 2 < MaxCol) { col++; v += new Vector3(0f, -14f, 0f); } 
                    break;
                case 2: // left
                    if (row - 2 >= 0) { row--; v += new Vector3(-25f, 0f, 0f); }
                    break;
                case 3: // right
                    if (row + 2 < MaxRow) { row++; v += new Vector3(25f, 0f, 0f); }
                    break;
            }
        }
        maps[row + col * MaxRow] = Instantiate(boss[round-1 % 3], v, Quaternion.identity);
        maps[row + col * MaxRow].name = row.ToString() + "." + col.ToString() + ".boss";
        map_info[row + col * MaxRow] = 2; // boss
    }

    public void NextRound()
    {
        if (round == MaxRound) { t_over.text = "게임 클리어!"; RewardManager.instance.ClearGame(); }
        else
        {
            round += 1; t_round.text = "Round " + round.ToString();
            RewardManager.instance.MoveChar();
            shop.transform.position = v;
            row = 3; col = 3; count = MaxCount; mapReset = false;
            MapClear(); GameStart();
        }
    }

    public void GameStart()
    {
        maps[row + col * MaxRow] = shop; map_info[row + col * MaxRow] = 3;
        t_over.text = "공략 실패!";
        isGameOver = false;
        MakeGround(); MakeMap();
        Invoke("asdf", 0.25f);
        soundManager.PlayBGM(1);
    }

    public void GameEnd()
    {
        round = 0; mob_cnt = 0; row = 3; col = 3; v = new Vector3(50f, 7f, 0f);
        t_round.text = "Round 1"; round += 1;
        MapClear(); isGameOver = true; mapReset = false;
        stat.SetActive(false); over.SetActive(true);
        soundManager.PlayBGM(0);
    }

    private void MapClear()
    {
        for (int i = 0; i < maps.Count; i++) if (map_info[i] != 3) { Destroy(maps[i]); maps[i] = null; }
        for (int i = 0; i < map_info.Count; i++) map_info[i] = 0;
        for (int i = 0; i < bodys.Count; i++) Destroy(bodys[i]); bodys.Clear();
    }

    public void asdf()
    {
        maps[row + col * MaxRow].GetComponent<GroundManager>().ClearMap();
    }

}