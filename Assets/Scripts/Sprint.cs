using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sprint : MonoBehaviour
{
    private ControllerMapping controls;
    private InputAction sprintAction;
    private Movement mov;

    public float sprintModifier;
    private float runSpeed;

    public bool isSprinting;

    public float smoothingModifier;
    public float smoothingRevertTime;
    private float originalSmoothing;

    void Awake()
    {
        controls = new ControllerMapping();
        mov = FindObjectOfType<Movement>();
    }

    private void OnEnable()
    {
        // set up jump
        sprintAction = controls.Player.Sprint;
        sprintAction.started += ctx => StartSprint();
        sprintAction.canceled += ctx => StopSprint();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        sprintAction.Disable();
    }

    private void StartSprint()
    {
        isSprinting = true;
        var pc = gameObject.GetComponent<Movement>();
        runSpeed = pc.runSpeed;
        pc.runSpeed = runSpeed * sprintModifier;
    }

    private void StopSprint()
    {
        isSprinting = false;
        var pc = gameObject.GetComponent<Movement>();
        pc.runSpeed = runSpeed;
        StartSmoothing();
        Invoke("StopSmoothing", smoothingRevertTime);
    }

    private void StartSmoothing()
    {
        mov.movementSmoothing = smoothingModifier;
    }

    private void StopSmoothing()
    {
        mov.movementSmoothing = originalSmoothing;
    }
}
