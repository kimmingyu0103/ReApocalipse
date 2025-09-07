using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldTextController : MonoBehaviour
{

    TextMeshPro text;
    Color alpha;
    public float gold;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        if (gold != 0) text.text = "+" + gold.ToString() + "°ñµå";
        Invoke("Destroy", 1f);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, 2f * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, 1f * Time.deltaTime);
        text.color = alpha;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
