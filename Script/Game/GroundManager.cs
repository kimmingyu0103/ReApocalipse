using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundManager : MonoBehaviour
{

    public GameObject[] potals;
    //public GameObject text;

    public GameObject[] mobs;
    //public GameObject reward;

    [SerializeField] int mapType;
    [SerializeField] bool cleared;

    void Start()
    {
        foreach(GameObject p in potals)
            p.SetActive(false);
    }
    
    public void GenerateMob()
    {
        if (!cleared)
        {
            Invoke("createMob", 0.25f);
        }
    }

    private void createMob()
    {
        if(mobs[0].name == "angel") { Instantiate(mobs[0], transform.position + new Vector3(0f, -7f, 0f), Quaternion.identity); return; }
        foreach (GameObject m in mobs)
            Instantiate(m, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-12f, -2f), 0f), Quaternion.identity);
        GameManager.instance.mob_cnt += mobs.Length;
    }

    public void ClearMap()
    {
        if(mapType==2) GameManager.instance.NextRound();
        if(!GameManager.instance.isGameOver) RewardManager.instance.GetReward();
        cleared = true; OpenPotals();
    }

    public void OpenPotals()
    {
        foreach (GameObject p in potals) p.SetActive(false);
        int row = GameManager.instance.row, col = GameManager.instance.col;
        int MaxRow = GameManager.instance.MaxRow, MaxCol = GameManager.instance.MaxCol;
        int i = row + col * MaxRow;

        if (col > 0) { // up
            if (GameManager.instance.maps[i - MaxRow] != null) {
                potals[0].SetActive(true);
            }
        }
        if (col + 1 < MaxCol) { // down
            if (GameManager.instance.maps[i + MaxRow] != null) {
                potals[1].SetActive(true);
            }
        }
        if (row > 0) { // left
            if (GameManager.instance.maps[i - 1] != null) {
                potals[2].SetActive(true);
            }
        }
        if (row + 1 < MaxRow) { // up
            if (GameManager.instance.maps[i + 1] != null) {
                potals[3].SetActive(true);
            }
        }

    }
    
}