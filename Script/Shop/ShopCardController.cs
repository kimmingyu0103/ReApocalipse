using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopCardController : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Text;
    [SerializeField] TMP_Text Price;
    [SerializeField] Texture2D cur;
    [SerializeField] Texture2D original;

    private Card c;

    public void SetUp(Card card)
    {

        c = card;

        img.sprite = c.sprite;
        Name.text = c.name;
        Text.text = c.text;
        Price.text = c.price.ToString() + "°ñµå";

    }

    private void OnMouseUpAsButton()
    {
        int r = Random.Range(0, 3);
        DeckManager.instance.pushDeck(c, r); RewardManager.instance.CardUpdate(c, r, c.price);
        transform.localScale *= 0.8f;
        this.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cur, Vector2.zero, CursorMode.ForceSoftware);
        transform.localScale *= 1.25f;
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.ForceSoftware);
        transform.localScale *= 0.8f;
    }

}