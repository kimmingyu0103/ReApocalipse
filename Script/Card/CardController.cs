using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Text;
    [SerializeField] TMP_Text Grade;
    [SerializeField] Texture2D cur;
    [SerializeField] Texture2D original;

    private Card c;

    private Vector3 originalScale;

    public void Start()
    {
        originalScale = transform.localScale;
    }

    public void SetUp(Card card)
    {

        c = card;
        
        img.sprite = c.sprite;
        Name.text = c.name;
        Text.text = c.text;
        Grade.text = c.grade;
    }

    private void OnMouseUpAsButton()
    {
        int r = Random.Range(0, 3);
        DeckManager.instance.pushDeck(c, r); RewardManager.instance.CardUpdate(c, r, 0);
        Time.timeScale = 1; transform.localScale = originalScale;
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cur, Vector2.zero, CursorMode.ForceSoftware);
        transform.localScale = originalScale * 1.25f;
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.ForceSoftware);
        transform.localScale = originalScale;
    }

}
