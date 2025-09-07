using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    [SerializeField] Renderer[] back;
    [SerializeField] Renderer[] middle;
    [SerializeField] string layer;
    int origin;

    void Start() { SetOrder(0); }

    public void SetOrigin(int origin) { this.origin = origin; SetOrder(origin); }

    public void SetMostFront(bool isFront) { SetOrder(isFront ? 100 : origin); }

    public void SetOrder(int order)
    {
        int mulOrder = order * 10;

        foreach(var r in back) { r.sortingLayerName = layer; r.sortingOrder = mulOrder; }

        foreach(var r in middle) { r.sortingLayerName = layer; r.sortingOrder = mulOrder + 5; }

    }

}
