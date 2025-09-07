using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobController : MonoBehaviour
{

    public GameObject DamageText;
    public GameObject GoldText;
    public GameObject body;
    private GameObject db;
    private Rigidbody2D rigid;
    private Animator ani;

    [SerializeField] float hp;
    [SerializeField] int mob_type;
    private bool isDeath = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        hp *= GameManager.instance.level * GameManager.instance.round; // 난이도 및 라운드 별 체력 세팅
        StartPatroll();
    }

    void Update()
    { // 맵 안에 가두기
        Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worldpos.x < 0.1f) worldpos.x = 0.1f;
        if (worldpos.y < 0.1f) worldpos.y = 0.1f;
        if (worldpos.x > 0.9f) worldpos.x = 0.9f;
        if (worldpos.y > 0.9f) worldpos.y = 0.9f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);
        if (GameManager.instance.isGameOver) Destroy(this.gameObject);
    }

    public void TakeDamage(float damage, float crtk) {

        if (hp <= 0) return;

        GameObject text = Instantiate(DamageText);
        text.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        if (crtk > Random.Range(0, 100))
        { text.GetComponent<DamageController>().crtk = true; damage *= 2; }
        text.GetComponent<DamageController>().damage = damage;

        hp -= damage; if(mob_type!=2) ani.SetTrigger("hit");
        if (hp <= 0)
        {
            GameManager.instance.MobDestroyed();
            isDeath = true; rigid.velocity = new Vector2(0f, 0f);
            if (mob_type != 2)
            {
                db = Instantiate(body);
                db.transform.position = transform.position;
                GameManager.instance.bodys.Add(db);
                Death();
            }
            else ani.SetTrigger("death");
        }
        if (mob_type == 0) StopCoroutine(PatrollSlime()); else StopCoroutine(PatrollSkeleton());
        rigid.velocity = new Vector2(1f, 0f);
    }

    private void Death()
    {
        GameObject gt = Instantiate(GoldText); int g = Random.Range(1, 6);
        gt.transform.position = transform.position + new Vector3(0f, 2.5f, 0f);
        gt.GetComponent<GoldTextController>().gold = g;
        RewardManager.instance.GetGold(g);
        Destroy(this.gameObject);
    }

    IEnumerator PatrollSlime()
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

    IEnumerator PatrollSkeleton()
    {
        while (!isDeath)
        {
            Vector2 v = Random.insideUnitCircle;
            rigid.velocity = v * 3;
            yield return new WaitForSeconds(Random.Range(3.0f, 10.0f));
            ani.SetTrigger("move");
        }
    }

    IEnumerator PatrollFly()
    {
        Vector2 v = Random.insideUnitCircle;
        while (!isDeath)
        {
            rigid.velocity = v * 3;
            yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
            v *= -1;
            yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        }
    }

    public void Move() { this.transform.position = transform.position + new Vector3(Random.Range(-7f,7f), Random.Range(-7f,7f), 0f); }
    public void DeathFly() { Death(); }
    public void StartPatroll() { if (mob_type == 0) StartCoroutine(PatrollSlime()); else if (mob_type == 1) StartCoroutine(PatrollSkeleton()); else StartCoroutine(PatrollFly()); }

}