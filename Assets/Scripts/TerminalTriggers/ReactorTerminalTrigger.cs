using Assets.Scripts.Dialog;
using Assets.Scripts.NameGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorTerminalTrigger : TerminalTrigger
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

            gameObject.GetComponent<DialogTrigger>().TriggerDialog();

            StartCoroutine(EnableDoorAndDisableBots());
        }
    }

    private IEnumerator EnableDoorAndDisableBots()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = replacement;

        var door = GameObject.FindGameObjectWithTag("EVADoor");
        door.GetComponent<Door>().doorActive = true;

        yield return null;
    }
}
