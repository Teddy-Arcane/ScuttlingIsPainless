using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public abstract class Jump: MonoBehaviour
    {
        public abstract void HandleJumping();
        public abstract void StartJump();
        public abstract void StopJump();
        public abstract bool CanJump();
    }
}