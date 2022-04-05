using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootPos;
    public float moveSpeed = 10f;

    public void Setup(Vector3 direction)
    {
        shootPos = direction;
        GetComponent<Rigidbody2D>().AddForce((direction - gameObject.transform.position) * moveSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetType() == typeof(CapsuleCollider2D))
            collision.gameObject.GetComponent<PlayerController>().Kill();
        if (collision.gameObject.tag != "RangedEnemy")
            Destroy(gameObject);
    }
}