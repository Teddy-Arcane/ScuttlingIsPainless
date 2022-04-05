using Assets.Scripts.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTerminal : MonoBehaviour
{
    internal bool triggerActive = false;

    private ControllerMapping controls;
    private InputAction interactAction;

    private void Awake()
    {
        controls = new ControllerMapping();
    }

    private void OnEnable()
    {
        // set up dash
        interactAction = controls.Player.Interact;
        interactAction.performed += ctx => SomeCoolAction();

        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    public void SomeCoolAction()
    {
        if (triggerActive)
        {
            gameObject.GetComponent<DialogTrigger>().TriggerDialog();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggerActive = true;
            PromptCanvasController.instance.SetText("Use", false);
            PromptCanvasController.instance.ToggleCanvas(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggerActive = false;
            PromptCanvasController.instance.ToggleCanvas(false);
        }
    }
}
