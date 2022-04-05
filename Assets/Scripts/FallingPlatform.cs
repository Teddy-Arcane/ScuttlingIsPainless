using Assets.Scripts.ShaderControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallTime;
    public float destroyTime;
    public float respawnTime;
    public string type;

    private bool isTriggered = false;
    private Rigidbody2D rb;
    private Vector3 originalPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = gameObject.GetComponentInParent<Transform>().position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.gameObject.tag == "Player")
        {
            isTriggered = true;
            Invoke("DropPlatform", fallTime);
        }
    }

    private void DropPlatform()
    {
        rb.isKinematic = false;

        GetComponent<Dissolve>().DoDissolve();

        Invoke("DestroyPlatform", destroyTime);
    }

    private void DestroyPlatform()
    {
        var data = new RespawnData(respawnTime, originalPosition, type);

        PlatformManager.instance.StartCoroutine("RespawnPlatform", data);

        Destroy(this);
        Destroy(gameObject);
        Destroy(gameObject.gameObject);
    }
}

public class RespawnData{
    public float respawnTime;
    public Vector3 pos;
    public string type;

    public RespawnData(float time, Vector3 p, string type)
    {
        pos = p;
        respawnTime = time;
        this.type = type;
    }
}