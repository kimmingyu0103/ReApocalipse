using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlimeController : MonoBehaviour
{

    public GameObject DamageText;
    public GameObject GoldText;
    private Rigidbody2D rigid;
    private Animator ani;

    private float MaxHp = 12;
    private float hp;
    private int level = 3;
    private bool isDeath = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        hp = MaxHp * GameManager.instance.level * GameManager.instance.round;
        StartCoroutine(Patroll());
    }

    void Update()
    {
        Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worldpos.x < 0.1f) worldpos.x = 0.1f;
        if (worldpos.y < 0.1f) worldpos.y = 0.1f;
        if (worldpos.x > 0.9f) worldpos.x = 0.9f;
        if (worldpos.y > 0.9f) worldpos.y = 0.9f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);
        if (GameManager.instance.isGameOver) Destroy(this.gameObject);
    }

    public void TakeDamage(float damage, float crtk) {


        GameObject text = Instantiate(DamageText);
        text.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        if (crtk > Random.Range(0, 100))
        { text.GetComponent<DamageController>().crtk = true; damage *= 2; }
        text.GetComponent<DamageController>().damage = damage;

        hp -= damage; ani.SetTrigger("hit");
        if (hp <= 0) { isDeath = true; Split(); }
        StopCoroutine(Patroll()); rigid.velocity = new Vector2(1f, 0f);
    }

    public void SetHp(float h, int l)
    {
        MaxHp = h / 2; hp = MaxHp * GameManager.instance.level * GameManager.instance.round; level=l-1; transform.localScale *= 0.5f;
    }

    public void Split()
    {
        if (level == 0) Death();
        else {
            Instantiate(this.gameObject).GetComponent<SlimeController>().SetHp(MaxHp, level);
            Instantiate(this.gameObject).GetComponent<SlimeController>().SetHp(MaxHp, level);
            GameManager.instance.mob_cnt++; Destroy(this.gameObject);
        }
    }

    private void Death()
    {
        GameManager.instance.MobDestroyed();
        GameObject gt = Instantiate(GoldText); int g = Random.Range(2, 10);
        gt.transform.position = transform.position + new Vector3(0f, 2.5f, 0f);
        gt.GetComponent<GoldTextController>().gold = g;
        RewardManager.instance.GetGold(g);
        Destroy(this.gameObject);
    }

    IEnumerator Patroll()
    {
        while (!isDeath) {
            Vector2 v = Random.insideUnitCircle;
            rigid.velocity = v * 5;
            ani.SetBool("isRun", true);
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
            rigid.velocity = v * 0;
            ani.SetBool("isRun", false);
            yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
        }
    }

    public void StartPatroll() { StartCoroutine(Patroll()); }

}