using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVATrigger : MonoBehaviour
{
    public bool eva;

    private PlayerController pc;

    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    private void TriggerEVA()
    {
        pc.ToggleGravity(eva);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            TriggerEVA();
        }
    }
}