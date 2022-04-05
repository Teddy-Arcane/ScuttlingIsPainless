using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZeroGravityMovement : MonoBehaviour
{
    private ControllerMapping controls;
    private InputAction moveAction;
    private InputAction thrustAction;
    private Rigidbody2D rb;

    public ParticleSystem up;
    public ParticleSystem down;
    public ParticleSystem left;
    public ParticleSystem right;

    public float thrustForce;

    public float maxVelocity;

    private bool isFacingRight = true;

    private void Awake()
    {
        controls = new ControllerMapping();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(rb.velocity.x >= maxVelocity)
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        if(rb.velocity.y >= maxVelocity)
            rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
        if(rb.velocity.x <= (maxVelocity * -1))
            rb.velocity = new Vector2((maxVelocity * -1), rb.velocity.y);
        if(rb.velocity.y <= (maxVelocity * -1))
            rb.velocity = new Vector2(rb.velocity.x, (maxVelocity * -1));
    }

    void OnEnable()
    {
        moveAction = controls.Player.Move;
        moveAction.Enable();

        thrustAction = controls.Player.Jump;
        thrustAction.Enable();
        thrustAction.performed += ctx => DoThrust();

        AudioManager.instance.StopSound("Walk");
        AudioManager.instance.StopSound("Sprint");

        var direction = moveAction.ReadValue<Vector2>();
        isFacingRight = direction.x > 0;
    }

    void OnDisable()
    {
        moveAction.Disable();
        thrustAction.Disable();
    }

    private void DoThrust()
    {
        var direction = moveAction.ReadValue<Vector2>();

        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
            Flip();

        if (direction.x > 0)
            left.Play();
        if(direction.x < 0)
            right.Play();
        if (direction.y > 0)
            down.Play();
        if (direction.y < 0)
            up.Play();

        AudioManager.instance.PlaySound("Thrust");

        rb.AddForce(direction * thrustForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
