using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StoneController : MonoBehaviour
{

    public GameObject DamageText;
    public GameObject GoldText;
    private Rigidbody2D rigid;
    //private Animator ani;

    private SpriteRenderer sr;
    [SerializeField] private Sprite[] DeathImg;
    //[SerializeField] public Sprite headImg;
    //[SerializeField] private GameObject next;
    //private int index;

    private Vector3 direction, pos;
    private float hp = 12;
    private bool isDeath = false;
    //[SerializeField] private bool isHead, isFirst;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //ani = GetComponent<Animator>();
        pos = transform.position;
        StartCoroutine(Patroll());
        hp *= GameManager.instance.level * GameManager.instance.round;
    }

    void FixedUpdate()
    {

        Vector3 f = direction - transform.position;
        f = f.normalized * 10f; rigid.AddForce(f);
        //transform.position = Vector3.Lerp(gameObject.transform.position, direction, 0.05f);
        //transform.position = Vector3.MoveTowards(gameObject.transform.position, Vector3.Lerp(gameObject.transform.position, direction, 0.1f), 0.1f);
    }

    void Update()
    {
        Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worldpos.x < 0.05f) worldpos.x = 0.05f;
        if (worldpos.y < 0.1f) worldpos.y = 0.1f;
        if (worldpos.x > 0.95f) worldpos.x = 0.95f;
        if (worldpos.y > 0.9f) worldpos.y = 0.9f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);
        if (GameManager.instance.isGameOver) Destroy(this.gameObject);
    }

    public void TakeDamage(float damage) {

        if (hp <= 0) return;

        GameObject text = Instantiate(DamageText);
        text.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        text.GetComponent<DamageController>().damage = damage;

        hp -= damage;
        if (hp <= 0) {
            GameManager.instance.MobDestroyed();
            direction = transform.position; isDeath = true;
            StartCoroutine(Death());
            //if(next!=null) next.GetComponent<StoneController>().NextHead();
        }
    }

    IEnumerator Death()
    {
        GameObject gt = Instantiate(GoldText); int g = Random.Range(10, 20);
        gt.transform.position = transform.position + new Vector3(0f, 2.5f, 0f);
        gt.GetComponent<GoldTextController>().gold = g;
        RewardManager.instance.GetGold(g);

        sr.sprite = DeathImg[0];
        yield return new WaitForSeconds(0.1f);
        sr.sprite = DeathImg[1];
        yield return new WaitForSeconds(0.1f);
        sr.sprite = DeathImg[2];
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    /*public void NextHead()
    {
        isHead = true; GetComponent<Image>().sprite = headImg; StartCoroutine(Patroll());
    }*/

    IEnumerator Patroll()
    {
        while (!isDeath) {
            direction = pos + new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0f);
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    public void StartPatroll() { StartCoroutine(Patroll()); }

}