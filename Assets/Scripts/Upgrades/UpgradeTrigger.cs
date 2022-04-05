using Assets.Scripts.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    public string upgradeName;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pc = GameObject.FindGameObjectWithTag("Player");

        if(collision.tag == "Player")
        {
            switch (upgradeName)
            {
                case "sprint":
                    pc.GetComponent<Sprint>().enabled = true;
                    break;
                case "dash":
                    pc.GetComponent<Dash>().enabled = true;
                    break;
                case "thrusters":
                    pc.GetComponent<ZeroGravityMovement>().enabled = true;
                    break;
                case "gun":
                    //pc.GetComponent<Gun>().enabled = true;
                    break;
                default:
                    break;
            }

            gameObject.GetComponent<DialogTrigger>().TriggerDialog();

            Destroy(this.gameObject);
        }
    }
}
