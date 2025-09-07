using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopperManager : MonoBehaviour
{

    [SerializeField] int shop_num;
    [SerializeField] Texture2D cur;
    [SerializeField] Texture2D original;

    public GameObject shop;

    void Start()
    {
        shop.SetActive(false);
    }

    void OnMouseUpAsButton()
    {
        OpenShop();
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cur, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnClickReturn()
    {
        shop.SetActive(false);
    }

    private void OpenShop()
    {
        shop.SetActive(true);
    }



}
