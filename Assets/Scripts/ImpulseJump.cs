using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ImpulseJump: Jump
{
    private Animator animator;

    private Movement controller;
    private Rigidbody2D rb;
    public Slider slider;
    
    public int maxJumpForce = 40;
    public int minJumpForce = 15;
    public float fallMultiplier = 3;
    public int maxPops = 2;
    public float jumpForce = 0.5f;
    public float maxVerticalVelocity = 0.5f;
    
    private int iniAirPops = 0;
    private int jumpForceCounter = 0;

    private bool jump;

    public float landingShakeDuration = 1f;
    
    private ControllerMapping controls;
    private InputAction jumpAction;

    private Sound charge;
    
    void Awake()
    {
        controls = new ControllerMapping();

        animator = GetComponent<Animator>();    
        
        controller = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
        
        slider.minValue = 0;
        slider.maxValue = 40;
        
        controller.OnLandEvent.AddListener(ResetPopCounter);
    }

    private void OnEnable()
    {
        charge = AudioManager.instance.GetSound("Charge");

        // set up jump
        jumpAction = controls.Player.Jump;
        jumpAction.started += ctx => StartJump();
        jumpAction.canceled += ctx => StopJump();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
    }

    private void ResetPopCounter()
    {
        FindObjectOfType<CameraShake>().ShakeCamera(.3f);
        iniAirPops = 0;
    }

    private void FixedUpdate()
    {
        if (DialogManager.instance.IsDialogShowing) return;

        if (jumpForceCounter > 0)
            jumpForceCounter++;
        
        slider.value = jumpForceCounter;

        if (jumpForceCounter > 10 && !charge.source.isPlaying && !(jumpForceCounter > maxJumpForce))
            charge.source.Play();

        HandleGravity();

        if (jump)
        {
            charge.source.Stop();
            HandleJumping();
        }
    }

    private void HandleGravity()
    {
        if (fallMultiplier > 0 && rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    public void KillJump()
    {
        jumpForceCounter = 0;
        jump = false;
    }

    public override bool CanJump()
    {
        return !(iniAirPops >= maxPops);
    }
    
    public override void StartJump()
    {
        if (!DialogManager.instance.IsDialogShowing)
            jumpForceCounter++;
    }
    
    public override void StopJump()
    {
        if (DialogManager.instance.IsDialogShowing || iniAirPops >= maxPops)
        {
            jumpForceCounter = 0;
            return;
        }
            
        if (!DialogManager.instance.IsDialogShowing)
            jump = true;
    }
    
    public override void HandleJumping()
    {
        if (iniAirPops >= maxPops)
            return;

        AudioManager.instance.PlaySound("Jump");

        if (jumpForceCounter > maxJumpForce)
            jumpForceCounter = maxJumpForce;

        if (jumpForceCounter < minJumpForce)
            jumpForceCounter = minJumpForce;

        controller.isOnGround = false;

        rb.AddForce(new Vector2(0f, jumpForce) * jumpForceCounter, ForceMode2D.Force);

        iniAirPops++;

        jump = false;

        jumpForceCounter = 0;
    }
}