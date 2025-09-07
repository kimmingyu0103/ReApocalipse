using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageController : MonoBehaviour
{

    void Start()
    { // ���� �� 3�ʰ� ���� ���� �߻�
        Invoke("DestroyBarrage", 5);
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 5;
    }

    private void OnTriggerEnter2D(Collider2D target) // Ÿ��
    { if (target.CompareTag("Player")) DestroyBarrage(); }

    private void DestroyBarrage() { Destroy(gameObject); }

}
