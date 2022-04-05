using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public float offset;
    public GameObject target;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(0, 
            new Vector3(
                transform.position.x,
                transform.position.y - offset,
                0f
            )
        );
        var hit = Physics2D.Linecast(transform.position, target.transform.position);

        if (hit.collider)
        {
            if (hit.collider.gameObject.tag == "Player" && hit.collider.GetType() == typeof(CapsuleCollider2D))
                hit.collider.gameObject.GetComponent<PlayerController>().Kill();

            _lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, 0f));
        }
        else
        {
            _lineRenderer.SetPosition(1, target.transform.position);
        }
    }
}
