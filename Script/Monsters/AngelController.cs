using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AngelController : MonoBehaviour
{
    [SerializeField] GameObject DamageText;
    [SerializeField] GameObject GoldText;
    [SerializeField] GameObject[] Patterns;

    private SpriteRenderer sr;
    [SerializeField] Sprite origin, attack;
    [SerializeField] Sprite[] bomb;

    private float hp = 24;
    private float hp2;
    private bool isDeath = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hp *= GameManager.instance.level * GameManager.instance.round; hp2 = hp / 2;
        StartCoroutine(Phase1());
    }

    public void TakeDamage(float damage)
    {
        GameObject text = Instantiate(DamageText);
        text.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        text.GetComponent<DamageController>().damage = damage;

        hp -= damage;
        if (hp <= 0) { isDeath = true; Death(); }
        else if (hp < hp2) { StopCoroutine(Phase1()); StartCoroutine(Phase2()); } // 2ÆäÀÌÁî
    }

    private void Death()
    {
        GameManager.instance.MobDestroyed();
        GameObject gt = Instantiate(GoldText); int g = Random.Range(30, 50);
        gt.transform.position = transform.position + new Vector3(0f, 2.5f, 0f);
        gt.GetComponent<GoldTextController>().gold = g;
        RewardManager.instance.GetGold(g);
        Invoke("Des", 1.2f);
    }
   
    private void Des() { Destroy(this.gameObject); }

    IEnumerator Phase1()
    {
        while (!isDeath)
        {
            StartCoroutine("Pattern" + Random.Range(0, 2)); sr.sprite = attack;
            yield return new WaitForSeconds(0.5f); sr.sprite = origin;
            yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
        }
    }

    IEnumerator Phase2()
    {
        while (!isDeath)
        {
            StartCoroutine("Pattern" + Random.Range(0, 3)); sr.sprite = attack;
            yield return new WaitForSeconds(0.5f); sr.sprite = origin;
            yield return new WaitForSeconds(Random.Range(1.5f, 3.5f));
        }
    }

    IEnumerator Pattern0()
    {
        for (int i = 0; i < 12; i++) { Instantiate(Patterns[0], transform.position, Quaternion.identity); yield return new WaitForSeconds(0.1f); }
    }

    IEnumerator Pattern1()
    {
        GameObject laser = Instantiate(Patterns[1], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f); laser.SetActive(false);
        yield return new WaitForSeconds(0.2f); laser.SetActive(true);
        yield return new WaitForSeconds(0.2f); Destroy(laser);
        yield return new WaitForSeconds(0.4f);
        GameObject laser2 = Instantiate(Patterns[2], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f); ; Destroy(laser2);
    }

    IEnumerator Pattern2()
    {
        for (int i = 0; i < 5; i++) {
            StartCoroutine(Pattern2_Sub()); yield return new WaitForSeconds(0.1f);
        }

    }

    IEnumerator Pattern2_Sub()
    {
        GameObject fire = Instantiate(Patterns[3], transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-7f, 7f), 0f), Quaternion.identity); 
        SpriteRenderer fs = fire.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(0.1f); fs.sprite = bomb[0];
        yield return new WaitForSeconds(0.1f); fs.sprite = bomb[1];
        yield return new WaitForSeconds(0.4f);
        GameObject fire2 = Instantiate(Patterns[4], fire.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f); Destroy(fire); Destroy(fire2);
    }
}
