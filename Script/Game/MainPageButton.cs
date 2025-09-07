using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPageButton : MonoBehaviour
{

    public GameObject page;
    public GameObject main;
    public GameObject normal;
    public GameObject option;
    public GameObject over;
    public GameObject stat;
    public GameObject startPosition;
    public GameObject player;

    void Start()
    {
        //main.SetActive(false);
        normal.SetActive(false);
        option.SetActive(false);
        over.SetActive(false);
        stat.SetActive(false);
        player.SetActive(false);
        //gameStart();
    }

    public void OnClickStoryBtn()
    {
        GameManager.instance.level = 0.5f;
        GameManager.instance.MaxRound = -1;
        gameStart();
    }

    public void OnClickNormalBtn() 
    {
        main.SetActive(false);
        normal.SetActive(true);
    }

    public void OnClickOptionBtn()
    {
        main.SetActive(false);
        option.SetActive(true);
    }

    public void OnClickReturn()
    {
        normal.SetActive(false);
        option.SetActive(false);
        if(GameManager.instance.isGameOver) main.SetActive(true);
    }

    public void OnClickNormal()
    {
        GameManager.instance.level = 2;
        GameManager.instance.count = 7;
        GameManager.instance.MaxCount = 7;
        gameStart();
    }

    public void OnClickEasy()
    {
        GameManager.instance.level = 1;
        GameManager.instance.count = 5;
        GameManager.instance.MaxCount = 5;
        gameStart();
    }

    public void OnClickHard()
    {
        GameManager.instance.level = 3;
        GameManager.instance.count = 9;
        GameManager.instance.MaxCount = 9;
        gameStart();
    }

    public void OnClickMain()
    {
        main.SetActive(true); over.SetActive(false);
    }
    private void gameStart()
    {
        Camera.main.GetComponent<MoveCamera>().Move(startPosition);
        player.SetActive(true);
        stat.SetActive(true);
        main.SetActive(false);
        normal.SetActive(false);
        GameManager.instance.isGameOver = false;
        GameManager.instance.GameStart();
    }

}
