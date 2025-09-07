using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterManager : MonoBehaviour
{

    // 캐릭터 및 UI 관련 스크립트
    private int curChar = 0;

    private Rigidbody2D rigid;
    private Animator ani;
    private SpriteRenderer sr;
    private Color alpha;
    float h; float v; // 이동 관련
    [SerializeField] [Range(0f, 20f)] float moveSpeed = 10f;

    private float curTime = 0f, coolTime = 0.1f; // 공격 관련

    public Transform pos_side; public Vector2 boxSize_side;
    public Transform pos_middle; public Vector2 boxSize_middle;

    public GameObject bullet;
    private Vector3 direction;

    private bool isAttack = false;
    private bool isCrash = false;

    public Image[] change_cool; // 교체 관련
    public TMP_Text[] change_time;
    private float changeCoolTime = 2f;
    public Character currentCharacter;
    public GameObject chall;
    [SerializeField] GameObject[] lock_img;
    [SerializeField] GameObject[] lock_img_chall;
    public bool[] char_lock;
    public bool isChange = false;

    private bool invenOn = false; // 인벤토리
    [SerializeField] GameObject inventory;

    private bool mapOn = false; // 맵
    [SerializeField] GameObject map;

    private bool setting = false; // 환경설정
    [SerializeField] GameObject settings;

    private float[] atk = { 5, 2, 2 }; // 스탯
    private int[] spd= { 30, 20, 5 };
    private int[] crtk = { 10, 5, 30 };
    private float[] gracePeriod = { 2f, 2f, 2f };
    private int[] lifeSteal = { 0, 0, 0 };
    private int hp = 5;
    private float dmg = 1;
    private int gold = 0;
    private int gold_rate = 1;
    [SerializeField] TMP_Text t_atk, t_gp, t_spd, t_crtk, t_gold;
    [SerializeField] GameObject[] hps;
    [SerializeField] GameObject DamageText;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        alpha = sr.color;

        inventory.SetActive(invenOn);
        map.SetActive(mapOn);
        settings.SetActive(setting);
        chall.SetActive(false);
        for (int i = 0; i < 3; i++) { lock_img[i].SetActive(false); lock_img_chall[i].SetActive(false); }

        UpdateStat();
    }

    void Update()
    {
        Inven();
        Map();
        Settings();
        Inside();
        Move();
        Attack();
        Change();
        Mode();
    }

    void FixedUpdate()
    {
        //공통
        h = Input.GetAxisRaw("Horizontal"); v = Input.GetAxisRaw("Vertical");
        if (!isAttack) rigid.velocity = new Vector2(h, v) * moveSpeed;

        // 반대로 그린 그림 뒤집기 용도
        if (h < 0) this.GetComponent<SpriteRenderer>().flipX = false;
        else if (h > 0) this.GetComponent<SpriteRenderer>().flipX = true;

        //징
        if (v != 0 || h != 0) direction = new Vector3(h, v, 0);

    }

    private void Move()
    {
        if (h == 0 && v == 0)
        {
            ani.SetInteger("run", 0);
        }
        else ani.SetInteger("run", 1);
    }

    private void Inven()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invenOn = !invenOn; inventory.SetActive(invenOn);
            DeckManager.instance.FreshInven();
        }
    }

    private void Map()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapOn = !mapOn; map.SetActive(mapOn);
            map.GetComponent<MapManager>().FreshMap();
        }
    }

    private void Mode()
    { // 개발자 모드
        if (Input.GetKeyDown(KeyCode.F))
        {
            atk = new float[] { 100, 100, 100 };
            spd = new int[] { 100, 100, 100 };
            gracePeriod = new float[] { 5f, 5f, 5f };
            UpdateStat();
        }
        if (Input.GetKeyDown(KeyCode.Q)) Application.Quit(); // 게임 종료
    }

    private void Settings()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!invenOn && !mapOn)
            {
                Time.timeScale = 0;
                if (!setting)
                {
                    settings.SetActive(true); setting = true;
                    //settings.GetComponent<RectTransform>().anchoredPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
                }
                else { settings.SetActive(false); Time.timeScale = 1; setting = false; }
            }

            else { invenOn = false; mapOn = false; inventory.SetActive(invenOn); map.SetActive(mapOn); }

        }
    }

    public void Lock() // 도전 성공 시 캐릭터 잠금
    {
        curChar = (int)currentCharacter; currentCharacter = (Character)3;
        char_lock[curChar] = true; lock_img[curChar].SetActive(true); lock_img_chall[curChar].SetActive(true); isChange = false;
        for (int i = 0; i < 3; i++) if (!char_lock[i]) lock_img[i].SetActive(false);
        for (int i = 0; i< 3; i++) {if (!char_lock[i]) { Invoke("Change" + i.ToString(), 0f); break; }}
    }

    public void ChallStart() // 보스 도전 중 캐릭터 교체 잠금
    {
        curChar = (int)currentCharacter; ani.SetTrigger("change" + curChar.ToString()); UpdateStat();
        if (curChar == 0) transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        else transform.localScale = new Vector3(1f, 1f, 1f);
        for (int i = 0; i < 3; i++) if (curChar != i) lock_img[i].SetActive(true);
        chall.SetActive(false); Time.timeScale = 1; isChange = true;
    }

    private void Change()
    {
        if (!isChange) {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !char_lock[0]) Change0();
            if (Input.GetKeyDown(KeyCode.Alpha2) && !char_lock[1]) Change1();
            if (Input.GetKeyDown(KeyCode.Alpha3) && !char_lock[2]) Change2();
        }
    }

    private void Change0()
    {
        ani.SetTrigger("change0");
        curChar = 0; UpdateStat();
        transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        if (!char_lock[1]) change_cool[1].fillAmount = 1;
        if (!char_lock[2]) change_cool[2].fillAmount = 1;
        StartCoroutine("ChangeCoolTime"); isChange = true;
    }

    private void Change1()
    {
        ani.SetTrigger("change1");
        curChar = 1; UpdateStat();
        transform.localScale = new Vector3(1f, 1f, 1f);
        if (!char_lock[0]) change_cool[0].fillAmount = 1;
        if (!char_lock[2]) change_cool[2].fillAmount = 1;
        StartCoroutine("ChangeCoolTime"); isChange = true;
    }

    private void Change2()
    {
        ani.SetTrigger("change2");
        curChar = 2; UpdateStat();
        transform.localScale = new Vector3(1f, 1f, 1f);
        if (!char_lock[0]) change_cool[0].fillAmount = 1;
        if (!char_lock[1]) change_cool[1].fillAmount = 1;
        StartCoroutine("ChangeCoolTime"); isChange = true;
    }

    IEnumerator ChangeCoolTime()
    {
        dmg *= 1.25f; // 캐릭터 교체시 최종뎀 1.25배
        while (changeCoolTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            changeCoolTime -= 0.1f; // Mathf.Round(coolTime*10)*0.1f
            for (int i = 0; i < 3; i++) {
                if (i != curChar) {
                    if(!char_lock[i]) change_cool[i].fillAmount = changeCoolTime / 2f;
                    if(!char_lock[i]) change_time[i].text = changeCoolTime.ToString("F1");
                }
            }
        } changeCoolTime = 2f;
        for(int i = 0;i<3; i++) change_time[i].text = "";
        isChange = false; dmg *= 0.8f;
    }

    public void Death()
    {
        GameManager.instance.GameEnd();
        this.transform.position = new Vector3(50f, 0f, 0f);
        transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        hp = 5; atk = new float[] { 5, 2, 2 }; // 스탯
        spd = new int[] { 30, 20, 5 };
        crtk = new int[] { 10, 5, 30 };
        gracePeriod = new [] { 2f, 2f, 2f };
        lifeSteal = new int[] { 0, 0, 0 };
        isAttack = false; isCrash = false;
        dmg = 1;
        gold = 0;
        gold_rate = 1;
        DeckManager.instance.ClearInven();
        ShopManager.instance.FreshShop();
        for (int i = 0; i < 3; i++) { char_lock[i] = false; lock_img[i].SetActive(false); lock_img_chall[i].SetActive(false); }
        for (int i = 0; i < 5; i++) hps[i].SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void UpdateStat()
    {
        t_atk.text = atk[curChar].ToString();
        t_gp.text = gracePeriod[curChar].ToString();
        t_spd.text = spd[curChar].ToString();
        t_crtk.text = crtk[curChar].ToString();
        t_gold.text = gold.ToString();
    }

    public void GetCardReward(int stat, int r, int g)
    {
        switch (stat)
        {
            case 0:
                atk[r]++; break;
            case 1:
                crtk[r] += 20; break;
            case 2:
                gold += 15 * gold_rate; break;
            case 3:
                gracePeriod[r] += 1f; break;
            case 4:
                spd[r] += 10; break;
            case 5:
                lifeSteal[r]++; break;
            case 6:
                for(int i = 0; i<3;i++) dmg *= 0.8f; gold_rate *= 2; break;
            case 7:
                for (int i = 0; i < 3; i++) dmg *= 1.25f; break;
        }

        gold -= g; UpdateStat();

    }

    public void DelCardReward(int stat, int r, int g)
    {
        switch (stat)
        {
            case 0:
                atk[r]--; break;
            case 1:
                crtk[r] -= 20; break;
            case 2:
                break;
            case 3:
                gracePeriod[r] -= 1f; break;
            case 4:
                spd[r] -= 10; break;
            case 5:
                lifeSteal[r]--; break;
            case 6:
                for (int i = 0; i < 3; i++) dmg *= 1.25f; gold_rate /= 2; break;
            case 7:
                for (int i = 0; i < 3; i++) dmg /= 0.8f; break;
        }

        gold += g; UpdateStat();

    }

    public void GetGold(int g)
    {
        gold += g * gold_rate;
        t_gold.text = gold.ToString();

    }

    public void HeartSteal()
    {
        if (hp != 5 && lifeSteal[curChar] * 3 > Random.Range(0, 100)) hps[hp++].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D target) // 피격
    {
        if (!isCrash && ((target.CompareTag("Enemy") || target.CompareTag("Stone") || target.CompareTag("Slime") || target.CompareTag("Pattern") )))
        {
            if (spd[curChar] < Random.Range(0, 100))
            {
                //ani.SetBool("Crash", true);
                isCrash = true;
                hps[--hp].SetActive(false);
                if (hp <= 0) Death();
                else StartCoroutine("Blink");
            }
            else { Instantiate(DamageText).transform.position = transform.position + new Vector3(0f, 2f, 0f); } // 빗나감 출력
        }
    }

    private void Inside()
    { // 맵 안에 가두기 
        if (!GameManager.instance.isMapMoving)
        {
            Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
            if (worldpos.x < 0f) worldpos.x = 0f;
            if (worldpos.y < 0f) worldpos.y = 0f;
            if (worldpos.x > 1f) worldpos.x = 1f;
            if (worldpos.y > 1f) worldpos.y = 1f;
            this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);
        }
    }

    IEnumerator Blink()
    { // 피격 후 무적 이펙트
        float blinkTime = gracePeriod[curChar];
        while (blinkTime >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            alpha.a = 0.5f;
            sr.color = alpha;
            yield return new WaitForSeconds(0.1f);
            alpha.a = 1f;
            sr.color = alpha;
            blinkTime -= 0.2f;
        }
        isCrash = false;
    }

    //공격
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && curTime <= 0f)
        {
            ani.SetTrigger("attack");
            curTime = coolTime; StartCoroutine(AttackCoolTime());

            //비
            if (curChar == 0)
            { rigid.velocity = new Vector2(5f, 0f); isAttack = true; }
            //징        총알에 현재 보고 있는 방향 정보 전달
            else if (curChar == 1) Instantiate(bullet, transform.position, Quaternion.Euler(direction)).GetComponent<BulletController>().SetAtk(atk[1] * dmg, crtk[1]);
            //주
            else
            {
                Collider2D[] c2d = Physics2D.OverlapBoxAll(pos_middle.position, boxSize_middle, 0);
                foreach (Collider2D collider in c2d)
                {
                    if (collider.CompareTag("Enemy")) collider.GetComponent<MobController>().TakeDamage(atk[2] * dmg, crtk[2]);
                    if (collider.CompareTag("Slime")) collider.GetComponent<SlimeController>().TakeDamage(atk[2] * dmg, crtk[2]);
                    if (collider.CompareTag("Stone")) collider.GetComponent<StoneController>().TakeDamage(atk[2] * dmg);
                    if (collider.CompareTag("Angel")) collider.GetComponent<AngelController>().TakeDamage(atk[2] * dmg);
                }

                curTime = coolTime; StartCoroutine(AttackCoolTime());
                //rigid.velocity *= 0;
                isAttack = true;
            }
        }

    }
    
    //비챤
    public void AttackStart() { StartCoroutine(AttackDash()); }
    public void AttackEnd() { isAttack = false; } // 비 주
    public void AttackDamage()
    {
        Vector3 pos = pos_side.position;
        if(sr.flipX) pos += new Vector3(1.75f, 0f, 0f);
        else pos -= new Vector3(1.75f, 0f, 0f);
        Collider2D[] c2d = Physics2D.OverlapBoxAll(pos, boxSize_side, 0);
        foreach (Collider2D collider in c2d)
        {
            if (collider.CompareTag("Enemy")) collider.GetComponent<MobController>().TakeDamage(atk[0] * dmg, crtk[0]);
            else if (collider.CompareTag("Slime")) collider.GetComponent<SlimeController>().TakeDamage(atk[0] * dmg, crtk[0]);
            else if (collider.CompareTag("Stone")) collider.GetComponent<StoneController>().TakeDamage(atk[0] * dmg);
            else if (collider.CompareTag("Angel")) collider.GetComponent<AngelController>().TakeDamage(atk[0] * dmg);
        }
    }

    IEnumerator AttackDash()
    {
        Vector2 v; float t = 0f;
        if (sr.flipX) v = new Vector2(5f, 0f);
        else v = new Vector2(-5f, 0f);

        while (t < 0.2f)
        {
            rigid.velocity = Vector2.Lerp(v, Vector2.zero, t / 0.2f);
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator AttackCoolTime()
    {
        while (curTime > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            curTime -= Time.deltaTime;
        }
        isAttack = false;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos_side.position, boxSize_side);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos_middle.position, boxSize_middle);
    }*/

}


/*

게임의 골자

3명의 캐릭터를 키워서 3마리 보스에게 도전한다.
상점과 몬스터 처치로 골드와 카드를 모은다.
단, 몬스터 처치 보상 카드는 랜덤한 캐릭터에게 지급된다.
상점은 원하는 캐릭터에게 구매 가능

보스방에 입장하면 해당 보스에서 사용할 캐릭터 1명을 고르고
보스를 처치하고 나면 해당 캐릭터는 사용하지 못한다.

적절한 리워드의 분배로 캐릭터를 전략적으로 키우는 게임
 
 */