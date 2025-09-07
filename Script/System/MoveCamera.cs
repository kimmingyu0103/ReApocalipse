using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    Transform target;
    private float t;

    public void Move(GameObject tf)
    {
        StopCoroutine(MoveCam());
        t = 0.0f; target = tf.transform;
        StartCoroutine(MoveCam());
    }

    IEnumerator MoveCam()
    {
        Vector3 v = transform.position;
        while (t < 3f)
        {
            if(t < 0.5f) GameManager.instance.isMapMoving = true;
            else GameManager.instance.isMapMoving = false;
            transform.position = Vector3.Lerp(transform.position, target.position, t / 3f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
