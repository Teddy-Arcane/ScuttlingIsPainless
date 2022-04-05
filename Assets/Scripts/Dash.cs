using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    private Movement movement;
    private Rigidbody2D rb;
    private ControllerMapping controls;
    
    public int maxDashes = 1;
    public float dashDistance = 15;
    public float dropDelay;

    public bool isDashing;
    private int inAirDashes = 0;
    
    private InputAction dashAction;
    private InputAction moveAction;

    private float initialGravity;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        controls = new ControllerMapping();
        rb = GetComponent<Rigidbody2D>();

        initialGravity = rb.gravityScale;

        movement.OnLandEvent.AddListener(ResetDashCount);
    }

    private void OnEnable()
    {
        moveAction = controls.Player.Move;

        // set up dash
        dashAction = controls.Player.Dash;
        dashAction.performed += ctx => StartDash();
        
        dashAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        dashAction.Disable();
        moveAction.Disable();
    }

    private void ResetDashCount()
    {
        inAirDashes = 0;
    }

    private void StartDash()
    {
        if (DialogManager.instance.IsDialogShowing) return;

        var direction = moveAction.ReadValue<Vector2>();

        var dir =
           direction.x > 0 ? 1
           : direction.x < 0 ? -1
           : 0;

        if (dir == 0)
            return;

        if (inAirDashes >= maxDashes)
            return;

        if (movement.isOnGround)
            return;

        isDashing = true;

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.AddForce(new Vector2(direction.x * dashDistance, 0f), ForceMode2D.Force);
        AudioManager.instance.PlaySound("Dash");

        inAirDashes++;

        Invoke("TurnShitBackOn", dropDelay);
    }

    private void TurnShitBackOn()
    {
        isDashing = false;
        rb.gravityScale = initialGravity;
    }
}
