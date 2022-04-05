using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public string id;
    public string goTo;

    bool active = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player"))
        {
            var warpTo = Resources.FindObjectsOfTypeAll<WarpPoint>()
                .FirstOrDefault(x => x.id == goTo);

            warpTo.active = false;

            collision.gameObject.GetComponent<Transform>().position = warpTo.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            active = true;
        }
    }
}