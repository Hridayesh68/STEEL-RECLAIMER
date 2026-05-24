using UnityEngine;
using UnityEngine.InputSystem;

namespace GinjaGaming.FinalCharacterController
{
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        public PlayerControls Controls { get; private set; }

        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool IsSprinting { get; private set; }

        private void OnEnable()
        {
            Controls = new PlayerControls();
            Controls.Enable();

            Controls.PlayerLocomotionMap.Enable();
            Controls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            Controls.PlayerLocomotionMap.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpPressed = context.performed;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            IsSprinting = context.ReadValueAsButton();
        }
    }
}