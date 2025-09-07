using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance { get; private set; }
    void Awake() => instance = this;

    [SerializeField] GameObject rewardUI; // 카드 보상
    [SerializeField] GameObject[] rewardSlot;
    [SerializeField] Card[] rewardCard;
    [SerializeField] GameObject Player;

    public int cur;
 
    public void Start()
    {
        rewardUI.SetActive(false);
    }

    public void GetReward()
    {
        Time.timeScale = 0;
        rewardUI.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            rewardCard[i] = CardManager.instance.PopCard();
            rewardSlot[i].GetComponent<CardController>().SetUp(rewardCard[i]);
        }
    }

    public void ClearGame()
    {
        Player.GetComponent<CharacterManager>().Death();
    }

    public void MoveChar()
    {
        Player.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 2.5f, 0f);
        Player.GetComponent<CharacterManager>().Lock();
    }

    public void CardUpdate(Card c, int r, int g) {
        Player.GetComponent<CharacterManager>().GetCardReward(c.stat, r, g); rewardUI.SetActive(false);
    }

    public void DelCardUpdate(Card c, int r, int g)
    {
        Player.GetComponent<CharacterManager>().DelCardReward(c.stat, r, g);
    }

    public void GetGold(int g)
    {
        Player.GetComponent<CharacterManager>().GetGold(g);
        if (g > 0) Player.GetComponent<CharacterManager>().HeartSteal();
    }

}
