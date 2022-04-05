using Assets.Scripts.Dialog;
using Assets.Scripts.NameGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class HubTerminalTrigger : TerminalTrigger
{
    public Sprite replacement;

    public bool done;

    private void Update()
    {
        if (done && triggerActive)
            PromptCanvasController.instance.ToggleCanvas(false);
    }

    public override void SomeCoolAction()
    {
        if (triggerActive)
        {
            if (done)
                return;

            done = true;

            Debug.Log("interacted with!");

            var lights = GameObject.FindGameObjectsWithTag("HubTurnGreen");
            foreach(var light in lights)
            {
                var flicker = light.GetComponent<Flicker>();
                flicker.enabled = false;
            }

            gameObject.GetComponent<DialogTrigger>().TriggerDialog();

            StartCoroutine(TurnOffAlarms(lights));
        }
    }

    private IEnumerator TurnOffAlarms(GameObject[] lights)
    {
        yield return new WaitForSeconds(3f);

        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = replacement;

        var door = GameObject.FindGameObjectWithTag("HubDoor");
        door.GetComponent<Door>().doorActive = true;

        AudioManager.instance.StopSound("Alarm");
        AudioManager.instance.PlaySound("AlarmDeath");

        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(2f);

        NameGenerator.instance.StartKilling();

        AudioManager.instance.PlaySound("MainLoop");

    }
}
