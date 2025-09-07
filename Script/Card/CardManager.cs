using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
     
    public static CardManager instance { get; private set; }
    void Awake() => instance = this;

    [SerializeField] CardSO CardList;
    [SerializeField] GameObject cardPrefeb;
    List<Card> cardBuffer;

    public void Setup()
    {
        cardBuffer = new List<Card>(); // 초기화 후 카드 넣기
        foreach(Card c in CardList.cards) { for (int i = 0; i < c.percent; i++) cardBuffer.Add(c); }
        for(int i = 0;i < cardBuffer.Count;i++) // 섞기
        {
            int r = Random.Range(i, cardBuffer.Count);
            Card tmp = cardBuffer[i]; cardBuffer[i] = cardBuffer[r]; cardBuffer[r] = tmp;
        }
    }

    public Card PopCard() // 카드 뽑기
    {
        if (cardBuffer.Count == 0) Setup(); 
        Card c = cardBuffer[0]; cardBuffer.RemoveAt(0);
        return c;
    }

    public void AddCard()
    {
        var card = Instantiate(cardPrefeb, Vector3.zero, Quaternion.identity);
        var c = card.GetComponent<CardController>();
        c.SetUp(PopCard());
    }

    void Start() { Setup(); }
}
