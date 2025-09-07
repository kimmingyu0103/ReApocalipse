using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Vector3 direction;
    public float distance;
    public LayerMask isLayer;
    private float atk;
    private float crtk;

    void Start()
    { 
        Invoke("DestroyBullet", 2);
        direction = transform.rotation.eulerAngles; // 캐릭터가 보고 있는 방향값 받아와서 확인
        if (direction.x == 359) direction.x = -1;
        if (direction.y == 359) direction.y = -1;
    }

    void Update()
    {
        // 투사체의 속도가 빠르므로 RayCast 사용
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, distance, isLayer);
        if(ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                ray.collider.GetComponent<MobController>().TakeDamage(atk, crtk);
                DestroyBullet();
            }
            else if (ray.collider.tag == "Slime")
            {
                ray.collider.GetComponent<SlimeController>().TakeDamage(atk, crtk);
                DestroyBullet();
            }
            else if (ray.collider.tag == "Stone")
            {
                ray.collider.GetComponent<StoneController>().TakeDamage(atk);
                DestroyBullet();
            }
            else if (ray.collider.tag == "Angel")
            {
                ray.collider.GetComponent<AngelController>().TakeDamage(atk);
                DestroyBullet();
            }
        }

        transform.Translate(direction * 20f * Time.deltaTime);
    }

    public void SetAtk(float a, float c) { atk = a; crtk = c; }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
