using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallGrab : MonoBehaviour
{
    private Movement movement;
    private ControllerMapping controls;
    private Rigidbody2D rb;
    public Transform frontChecker;
    public float wallSlideSpeed = 0.25f;
    public bool wallGrabbing;
    private bool isTouchingFront;

    private InputAction grabAction;
    
    private void Awake()
    {
        movement = GetComponent<Movement>();
        controls = new ControllerMapping();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        grabAction = controls.Player.Grab;
        grabAction.started += ctx => HandleGrabControl(true); 
        grabAction.canceled += ctx => HandleGrabControl(false); 
        grabAction.Enable();
    }

    private void OnDisable()
    {
        grabAction.Disable();
    }

    private void Update()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontChecker.position, 0.1f, movement.whatIsGround);
    }

    private void FixedUpdate()
    {
        if (DialogManager.instance.IsDialogShowing) return;

        if (isTouchingFront && !movement.isOnGround && wallGrabbing)
            HandleWallSliding();
    }
    
    private void HandleGrabControl(bool grab)
    {
        wallGrabbing = grab;
    }

    private void HandleWallSliding()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
    }
}