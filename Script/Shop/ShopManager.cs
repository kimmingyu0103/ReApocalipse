using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public static ShopManager instance { get; private set; }
    void Awake() => instance = this;

    [SerializeField] List<Card> shopCard;
    //[SerializeField] List<Item> shopItem;

    [SerializeField] GameObject[] slot;

    void Start()
    {
        FreshShop();
    }

    public void FreshShop()
    {
        shopCard = new List<Card>();
        for (int i = 0; i < 6; i++)
        {
            slot[i].SetActive(true);
            shopCard.Add(CardManager.instance.PopCard());
            slot[i].GetComponent<ShopCardController>().SetUp(shopCard[i]);
        }
    }

    public void RerollShop()
    {
        RewardManager.instance.GetGold(-5); FreshShop();
    }  


}
