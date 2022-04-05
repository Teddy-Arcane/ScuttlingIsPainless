using Assets.Scripts.Dialog;
using Assets.Scripts.ShaderControl;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private ControllerMapping controls;
    private InputAction gravityAction;
    private Rigidbody2D rb;

    public Vector3 checkpoint;
    
    void Awake()
    {
        controls = new ControllerMapping();
        rb = GetComponent<Rigidbody2D>();

        AudioManager.instance.PlaySound("Alarm");
        AudioManager.instance.PlaySound("Ambience");

        Application.targetFrameRate = 60;

        GetComponent<DialogTrigger>().TriggerDialog();
    }

    void Update()
    {
        if (DialogManager.instance.IsDialogShowing) return;

        //if (GetComponent<Transform>().position.y < -35)
        //    Kill();
    }

    public void ToggleGravity(bool isEva)
    {
        // turn off gravity
        if (isEva)
        {
            rb.gravityScale = 0;

            gameObject.GetComponent<ImpulseJump>().enabled = false;
            gameObject.GetComponent<Dash>().enabled = false;
            gameObject.GetComponent<Sprint>().enabled = false;
            gameObject.GetComponent<Movement>().enabled = false;

            gameObject.GetComponent<ZeroGravityMovement>().enabled = true;
        }
        else
        {
            rb.gravityScale = 3;

            gameObject.GetComponent<ImpulseJump>().enabled = true;
            gameObject.GetComponent<Dash>().enabled = true;
            gameObject.GetComponent<Sprint>().enabled = true;
            gameObject.GetComponent<Movement>().enabled = true;

            gameObject.GetComponent<ZeroGravityMovement>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (DialogManager.instance.IsDialogShowing) return;
    }

    public void Kill()
    {
        if (GetComponent<Dash>().isDashing)
            return;

        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;

        AudioManager.instance.PlaySound("Hit");

        Dissolve();
    }

    private void Dissolve()
    {
        AudioManager.instance.PlaySound("Dissolve");
        GetComponent<Dissolve>().DoDissolve();
        Invoke("MoveToCheckPoint", 1f);
    }

    private void MoveToCheckPoint()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Transform>().position = checkpoint;

        transform.parent = transform;

        UnDissolve();
    }

    private void UnDissolve()
    {
        AudioManager.instance.PlaySound("Undissolve");
        GetComponent<Dissolve>().RevertDissolve();
    }
}