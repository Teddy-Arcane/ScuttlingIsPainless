using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
	private Animator animator;

	public float movementSmoothing = 0.05f;
	public bool airControlEnabled = true;
	public LayerMask whatIsGround; 
    public Transform groundChecker;                        
	
	public bool isOnGround;            
	private Rigidbody2D rb;
	private bool isFacingRight = true;  
	private Vector3 velocity = Vector3.zero;

	public float runSpeed = 40f;
	float horizontalMove = 0f;

	public UnityEvent OnLandEvent;

	private Sprint sprint;

	private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

	private ControllerMapping controls;
	private InputAction moveAction;

	private Sound walkSound;
	private Sound sprintSound;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		animator = GetComponent<Animator>();

		sprint = GetComponent<Sprint>();

		controls = new ControllerMapping();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		walkSound = AudioManager.instance.GetSound("Walk");
		sprintSound = AudioManager.instance.GetSound("Sprint");
	}

	private float GetHorizontalMoveSpeed()
    {
		if (DialogManager.instance.IsDialogShowing)
			return 0f;

		return moveAction.ReadValue<Vector2>().x * runSpeed;
	}

    private void FixedUpdate()
    {
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		HandleCharacterControl(horizontalMove * Time.fixedDeltaTime);

		animator.SetBool("IsJumping", !isOnGround);

		HandleSound();
	}

	private void HandleSound()
    {
		CheckIfOnGround();

		horizontalMove = GetHorizontalMoveSpeed();

		if ((horizontalMove > 0.2 || horizontalMove < -0.2) && isOnGround)
		{
            if (sprint.isSprinting && !sprintSound.source.isPlaying)
            {
				sprintSound.source.Play();
				walkSound.source.Stop();
			}
			else if (!sprint.isSprinting && !walkSound.source.isPlaying)
            {
				sprintSound.source.Stop();
				walkSound.source.Play();
			}
		}
		else
		{
			sprintSound.source.Stop();
			walkSound.source.Stop();
		}
	}

    void OnEnable()
	{
		moveAction = controls.Player.Move;
		moveAction.Enable();
	}

	void OnDisable()
	{
		moveAction.Disable();
	}

	private void CheckIfOnGround()
	{
		if (!isOnGround)
			sw.Start();

		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundChecker.position, .1f, whatIsGround);

		if(colliders.Length > 0)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					isOnGround = true;
					if (sw.ElapsedMilliseconds > (long)60)
					{
						AudioManager.instance.PlaySound("Land");
						OnLandEvent.Invoke();
						sw.Reset();
					}
				}
			}
		}
		else
		{
			isOnGround = false;
		}
	}

	public void HandleCharacterControl(float move)
	{
		if (DialogManager.instance.IsDialogShowing)
        {
			StopMoving();
			return;
		}
			
		if ((isOnGround || (airControlEnabled))) 
			HandleMovement(move);
	}

	private void StopMoving()
    {
		Vector3 targetVelocity = new Vector2(0f, 0f);
		rb.velocity = targetVelocity;
	}

	private void HandleMovement(float move)
    {
		Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

		if ((move > 0 && !isFacingRight) || (move < 0 && isFacingRight))
			Flip();
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}