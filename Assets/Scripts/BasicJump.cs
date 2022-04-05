using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class BasicJump: Jump
    {
        private Movement controller;
        private Rigidbody2D rb;
        
        private ControllerMapping controls;
        private InputAction jumpAction;
        
        public float fallMultiplier = 3;
        public float lowJumpMultiplier = 3;
        private int iniAirPops = 0;
        private bool jump;
        public float jumpForce = 0.5f;
        public int maxPops = 2;
        private bool stopJump;
        
        void Awake()
        {
            controls = new ControllerMapping();
        
            controller = GetComponent<Movement>();
            rb = GetComponent<Rigidbody2D>();
        
            controller.OnLandEvent.AddListener(LandingEvent);
        }

        private void FixedUpdate()
        {
            if (DialogManager.instance.IsDialogShowing) return;

            if (jump) HandleJumping();

            HandleGravity();
        }
        
        private void OnEnable()
        {
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
        
        private void LandingEvent()
        {
            iniAirPops = 0;
            stopJump = false;
        }
        
        private void HandleGravity()
        {
            if (fallMultiplier > 0 && rb.velocity.y < 0)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            else if (lowJumpMultiplier > 0 && rb.velocity.y > 0 && stopJump)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
        public override void HandleJumping()
        {
            if (iniAirPops >= maxPops)
                return;
            
            controller.isOnGround = false;
        
            rb.AddForce(new Vector2(0f, jumpForce));

            iniAirPops++;

            jump = false;
        }

        public override void StartJump()
        {
            jump = true;
            stopJump = false;
        }

        public override void StopJump()
        {
            stopJump = true;
        }

        public override bool CanJump()
        {
            return !(iniAirPops >= maxPops);
        }
    }
}