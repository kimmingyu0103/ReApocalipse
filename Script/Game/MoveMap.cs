using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{

    public GameObject cam;
    public GameObject loc;
    public int direction;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Player"))
        {
            Camera.main.GetComponent<MoveCamera>().Move(cam);
            GameManager.instance.Move(direction);
            target.transform.position = loc.transform.position;
        }
    }

}
