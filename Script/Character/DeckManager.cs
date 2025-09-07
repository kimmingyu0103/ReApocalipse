using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance { get; private set; }
    void Awake() => instance = this;

    [SerializeField] CardSO CardList;

    [SerializeField] List<Card> vii;
    [SerializeField] List<Card> jing;
    [SerializeField] List<Card> zrr;

    [SerializeField] GameObject[] slot; // 인벤
    [SerializeField] TMP_Text CharName;

    private int cnt = 0;
    private int MaxCount = 30;
    private int CurChar = 0; // vii = 0, jing = 1, zrr = 2

    void Start()
    {
        vii = new List<Card>(); jing = new List<Card>(); zrr = new List<Card>(); // 초기화 후 카드 넣기
        //foreach (Card c in CardList.cards) { for (int i = 0; i < c.percent; i++) { vii.Add(c); jing.Add(c); zrr.Add(c); } }
        FreshInven(); 
    }

    public void pushDeck(Card card, int rand)
    {
        switch (rand)
        {
            case 0: Add(ref vii, card); break;
            case 1: Add(ref jing, card); break;
            case 2: Add(ref zrr, card); break;
        }
    }

    private void Add(ref List<Card> c, Card card)
    {
        if (c.Count < MaxCount) 
        {
            c.Add(card);

        }
    }

    public void OnClickAdd()
    {
        switch (CurChar)
        {
            case 0: Add(ref vii); break;
            case 1: Add(ref jing); break;
            case 2: Add(ref zrr); break;
        }
    }

    private void Add(ref List<Card> c)
    {
        if (c.Count < MaxCount)
        {
            c.Add(CardManager.instance.PopCard());
            slot[cnt].SetActive(true);
            slot[cnt].GetComponent<CardController>().SetUp(c[cnt++]);
            RewardManager.instance.CardUpdate(c[cnt-1], CurChar, 20);
        }
    }

    public void OnClickDelete()
    {
        switch (CurChar)
        {
            case 0: Del(ref vii); break;
            case 1: Del(ref jing); break;
            case 2: Del(ref zrr); break;
        }
    }

    private void Del(ref List<Card> c)
    {
        if(c.Count > 0)
        {
            RewardManager.instance.DelCardUpdate(c[c.Count - 1], CurChar, 10);
            c.RemoveAt(c.Count - 1);
            slot[--cnt].SetActive(false);
        }
    }

    public void OnClickNext()
    {
        CurChar++; CurChar %= 3; FreshInven();
    }

    public void OnClickBack()
    {
        CurChar += 2; CurChar %= 3; FreshInven();
    }

    public void FreshInven()
    {
        List<Card> tmp = new List<Card>();
        switch (CurChar) {
            case 0: tmp = vii; CharName.text = "비챤"; break;
            case 1: tmp = jing; CharName.text = "징버거"; break;
            case 2: tmp = zrr; CharName.text = "주르르"; break;
        }

        cnt = tmp.Count;
        for (int i = 0; i < cnt; i++) {
            slot[i].SetActive(true); 
            slot[i].GetComponent<CardController>().SetUp(tmp[i]);
        }
        for(int i = cnt; i < slot.Length; i++) { slot[i].SetActive(false); }

    }

    public void ClearInven()
    {
        vii.Clear(); jing.Clear(); zrr.Clear();
    }

    /*public int[] FreshStat(int character)
    {
        int[] stats = { 0, 0, 0, 0, 0, 0 }; // 0 공격 1 무적 2 속도 3 크리 4 피흡 5 골드 // 몹피

        switch (character)
        {
            case 0: 
                foreach (Card c in vii) { if (c.name == "무기 강화") stats[0]++; }
                break;
            case 1: Del(ref jing); break;
            case 2: Del(ref zrr); break; 
        }

        return stats;
    }*/
}