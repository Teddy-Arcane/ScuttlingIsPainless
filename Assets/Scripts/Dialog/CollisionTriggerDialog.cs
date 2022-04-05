using Assets.Scripts.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTriggerDialog : MonoBehaviour
{
    private DialogTrigger dialog;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !triggered)
        {
            dialog.TriggerDialog();
            triggered = true;
        }
    }

    void Awake()
    {
        dialog = GetComponent<DialogTrigger>();
    }
}
