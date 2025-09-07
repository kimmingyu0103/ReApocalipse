using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageController : MonoBehaviour
{

    TextMeshPro text;
    Color alpha;
    public float damage;
    public bool crtk = false;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        if(damage!=0) text.text = damage.ToString();
        if (crtk) text.text += "!";
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
