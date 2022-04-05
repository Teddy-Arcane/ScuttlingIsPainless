using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RangedEnemy : MonoBehaviour
{
    bool isAiming;
    LineRenderer lineRenderer;
    GameObject aimingAt;
    bool playerInRange;

    public Transform bullet;

    public Sprite friendly;
    public Sprite mad;
    public Light2D light;

    public float fireDelay;
    public float fireTime;
    public float timeBetweenShots;

    bool doDraw = false;

    public bool cancel = false;

    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();

        lineRenderer.enabled = false;

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!playerInRange && collision.gameObject.tag == "Player")
        {
            aimingAt = collision.gameObject;
            isAiming = true;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        { 
            playerInRange = false;
            cancel = true;
        }
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(lineRenderer.gameObject.transform.position.x, lineRenderer.gameObject.transform.position.y, 0f));

        if (cancel)
        {
            Cancel();
            return;
        }

        if (isAiming)
        {
            doDraw = true;
            lineRenderer.enabled = true;
            DrawLine();
            isAiming = false;
            light.color = Color.red;
            GetComponentInChildren<SpriteRenderer>().sprite = mad;
        }

        if (doDraw)
        {
            lineRenderer.SetPosition(1, new Vector3(aimingAt.transform.position.x, aimingAt.transform.position.y + 0.3f, 0f));
        }
    }

    private void Cancel()
    {
        CancelInvoke("Fire");
        CancelInvoke("EndFire");
        CancelInvoke("MaybeTryAgain");

        AudioManager.instance.GetSound("Laser").source.Stop();

        playerInRange = false;
        lineRenderer.enabled = false;
        GetComponentInChildren<SpriteRenderer>().sprite = friendly;
        isAiming = false;
        cancel = false;
        doDraw = false;
    }

    private void DrawLine()
    {
        if (cancel)
        {
            Cancel();
            return;
        }

        AudioManager.instance.PlaySound("Laser");
        Invoke("Fire", fireDelay);
    }

    private void Fire()
    {
        if (cancel)
        {
            Cancel();
            return;
        }

        lineRenderer.enabled = false;

        var trans = Instantiate(
            bullet,
            new Vector3(lineRenderer.gameObject.transform.position.x, lineRenderer.gameObject.transform.position.y, 0f),
            Quaternion.identity
        );

        var pos = lineRenderer.GetPosition(1);
        trans.GetComponent<Bullet>().Setup(new Vector3(aimingAt.transform.position.x, aimingAt.transform.position.y + 0.3f, 0f));

        Invoke("EndFire", fireDelay);

        doDraw = false;
    }

    private void EndFire()
    {
        if (cancel)
        {
            Cancel();
            return;
        }

        lineRenderer.enabled = false;

        light.color = Color.green;

        GetComponentInChildren<SpriteRenderer>().sprite = friendly;

        Invoke("MaybeTryAgain", timeBetweenShots);
    }

    private void MaybeTryAgain()
    {
        if (cancel)
        {
            Cancel();
            return;
        }

        if (playerInRange)
        {
            isAiming = true;
        }
    }
}
