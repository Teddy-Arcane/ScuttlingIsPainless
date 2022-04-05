using Assets.Scripts.Dialog;
using Assets.Scripts.NameGenerator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public bool doorActive;
    public bool triggerActive;
    public Light2D light;

    public bool isEVA;

    private BoxCollider2D bc;
    private ControllerMapping controls;
    private InputAction interactAction;

    private Animator animator;

    private Sound mainTheme;
    private Sound evaTheme;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controls = new ControllerMapping();
        bc = GetComponents<BoxCollider2D>().First(x => !x.isTrigger);

        mainTheme = AudioManager.instance.GetSound("MainLoop");
        evaTheme = AudioManager.instance.GetSound("EVA");
    }

    private void OnEnable()
    {
        interactAction = controls.Player.Move;

        interactAction = controls.Player.Interact;
        interactAction.performed += ctx => ToggleDoor();

        interactAction.Enable();
    }

    private void FixedUpdate()
    {
        if (light.intensity == 2)
            return;

        if (!bc.enabled)
        {
            PromptCanvasController.instance.ToggleCanvas(false);
        }

        light.intensity = doorActive ? 2 : 0;
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (bc.enabled && other.CompareTag("Player"))
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

    public void ToggleDoor() 
    {
        if (doorActive && triggerActive)
        {
            animator.SetBool("IsOpen", true);

            var colliders = GetComponents<BoxCollider2D>();
            foreach(var collider in colliders)
                collider.enabled = false;

            AudioManager.instance.PlaySound("DoorOpen");

            GetComponent<DialogTrigger>()?.TriggerDialog();

            if (isEVA)
            {
                mainTheme.source.Stop();
                evaTheme.source.Play();

                NameGenerator.instance.KillFaster();
            }
        }
        else if(!doorActive && triggerActive)
        {
            AudioManager.instance.PlaySound("Fail");
        }
    }
}
