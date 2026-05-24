using UnityEngine;

namespace GinjaGaming.FinalCharacterController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        private CharacterController controller;
        private PlayerLocomotionInput input;

        [Header("Movement")]
        public float moveSpeed = 5f;
        public float sprintSpeed = 8f;
        public float jumpHeight = 1.5f;
        public float gravity = -9.81f;

        [Header("Mouse Look")]
        public float mouseSensitivity = 100f;
        public float verticalClamp = 80f;

        private float yVelocity;
        private float xRotation = 0f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerLocomotionInput>();
        }

        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
        }

        // ---------------- MOVEMENT ----------------
        private void HandleMovement()
        {
            Vector2 moveInput = input.MovementInput;

            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

            float speed = input.IsSprinting ? sprintSpeed : moveSpeed;

            if (controller.isGrounded && yVelocity < 0)
                yVelocity = -2f;

            // Jump
            if (input.JumpPressed && controller.isGrounded)
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Gravity
            yVelocity += gravity * Time.deltaTime;

            Vector3 finalMove = move * speed;
            finalMove.y = yVelocity;

            controller.Move(finalMove * Time.deltaTime);
        }

        // ---------------- MOUSE LOOK ----------------
        private void HandleMouseLook()
        {
            Vector2 look = input.LookInput * mouseSensitivity * Time.deltaTime;

            xRotation -= look.y;
            xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * look.x);
        }
    }
}