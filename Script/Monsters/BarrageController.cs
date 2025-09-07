using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageController : MonoBehaviour
{

    void Start()
    { // 생성 후 3초간 랜덤 방향 발사
        Invoke("DestroyBarrage", 5);
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 5;
    }

    private void OnTriggerEnter2D(Collider2D target) // 타격
    { if (target.CompareTag("Player")) DestroyBarrage(); }

    private void DestroyBarrage() { Destroy(gameObject); }

}
