using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCollider : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType() == typeof(CapsuleCollider2D))
        {
            collision.gameObject.GetComponent<PlayerController>().Kill();
        }
    }
}
