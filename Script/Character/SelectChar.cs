using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    Vli, Jing, Zu, None
}

public class SelectChar : MonoBehaviour
{

    public Character character;
    public GameObject CharManager;
    Animator ani;
    SpriteRenderer sr;
    public SelectChar[] chars;
    private Color originColor, halfColor;

    void Start()
    {
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originColor = sr.color; halfColor = originColor * 0.5f;
        if (CharManager.GetComponent<CharacterManager>().currentCharacter == character) OnSelect();
        else OnDeSelect();
    }

    private void OnMouseUpAsButton()
    {
        CharManager.GetComponent<CharacterManager>().currentCharacter = character;
        OnSelect();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != this) chars[i].OnDeSelect();
        }
    }

    private void OnSelect()
    {
        if (!CharManager.GetComponent<CharacterManager>().char_lock[(int)character])
        {
            ani.SetBool("Run", true);
            sr.color = originColor;
        }
    }

    public void OnDeSelect()
    {
        ani.SetBool("Run", false); sr.color = halfColor;
    }

}
