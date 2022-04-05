using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TerminalTrigger : MonoBehaviour
{
    internal bool triggerActive = false;
    private ControllerMapping controls;
    private InputAction interactAction;
    private bool done;

    public string text;

    private void Awake()
    {
        controls = new ControllerMapping();
    }

    private void OnEnable()
    {
        interactAction = controls.Player.Move;

        // set up dash
        interactAction = controls.Player.Interact;
        interactAction.performed += ctx => SomeCoolAction();

        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && done == false)
        {
            triggerActive = true;
            PromptCanvasController.instance.SetText(text, false);
            PromptCanvasController.instance.ToggleCanvas(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggerActive = false;

            if(!done)
                PromptCanvasController.instance.ToggleCanvas(false);
        }
    }

    public abstract void SomeCoolAction();
}
