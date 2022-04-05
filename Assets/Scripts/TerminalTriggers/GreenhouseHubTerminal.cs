using Assets.Scripts.Dialog;
using Assets.Scripts.NameGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenhouseHubTerminal : TerminalTrigger
{
    public Sprite replacement;

    public bool done;

    public override void SomeCoolAction()
    {
        if (triggerActive)
        {
            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = replacement;

            gameObject.GetComponent<DialogTrigger>().TriggerDialog();

            NameGenerator.instance.KillFaster();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
