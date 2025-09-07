using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{ // 지도 관리

    [SerializeField] GameObject[] map;
    // row.col => index = col * 7 + row

    public Sprite none;
    public Sprite normal;
    public Sprite now;
    public Sprite boss;
    public Sprite shop;

    public void ResetMap()
    { GameManager.instance.mapReset = true; for (int i = 0; i<map.Length; i++) { Color c = map[i].GetComponent<Image>().color; c.a = 1f; map[i].GetComponent<Image>().color = c; }}

    public void FreshMap() {

        if (!GameManager.instance.mapReset) ResetMap();
        List<int> tmp = GameManager.instance.map_info;

        int cnt = tmp.Count;
        for (int i = 0; i < cnt; i++) {
            if (tmp[i] == 1) map[i].GetComponent<Image>().sprite = normal;
            else if (tmp[i] == 2) map[i].GetComponent<Image>().sprite = boss;
            else if (tmp[i] == 3) map[i].GetComponent<Image>().sprite = shop;
            else { Color c = map[i].GetComponent<Image>().color; c.a = 0f; map[i].GetComponent<Image>().color = c; }
            //else map[i].GetComponent<Image>().sprite = none;
        }

        map[GameManager.instance.row + GameManager.instance.col * GameManager.instance.MaxRow].GetComponent<Image>().sprite = now;
    }
}