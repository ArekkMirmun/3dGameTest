using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public float lookSensitivity = 1f;

    private CharacterController characterController;

    private Vector2 inputDirection;
    private Vector2 lookInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSliding;
    private Vector3 hitNormal;
    private Transform platform; // Current platform
    private Vector3 platformPreviousPosition;
    private Vector3 platformVelocity;
    [SerializeField] PickUp pickUp;

    private float xRotation = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckSlope();
        UpdatePlatformVelocity();
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = Vector3.zero;

        if (isSliding)
        {
            SlideDown();
        }
        else
        {
            move = new Vector3(inputDirection.x, 0, inputDirection.y);
            move = transform.TransformDirection(move) * moveSpeed;
        }

        // Add platform velocity if grounded on a platform
        if (isGrounded && platform != null)
        {
            move += platformVelocity;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Final movement
        characterController.Move((move + velocity) * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        transform.Rotate(0, lookInput.x * lookSensitivity * Time.deltaTime, 0);

        xRotation -= lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        UnityEngine.Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void CheckSlope()
    {
        float slopeAngle = Vector3.Angle(Vector3.up, hitNormal);
        isSliding = isGrounded && slopeAngle > characterController.slopeLimit && slopeAngle < 89f;
    }

    private void SlideDown()
    {
        Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hitNormal);
        characterController.Move(slideDirection * moveSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

        // Check if the hit object is a platform
        if (hit.collider.CompareTag("Platform"))
        {
            platform = hit.collider.transform;
            platformPreviousPosition = platform.position;
        }
        else
        {
            platform = null;
        }
    }

    private void UpdatePlatformVelocity()
    {
        if (platform != null)
        {
            platformVelocity = (platform.position - platformPreviousPosition) / Time.deltaTime;
            platformPreviousPosition = platform.position;
        }
        else
        {
            platformVelocity = Vector3.zero;
        }
    }

    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (isGrounded && value.isPressed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
    public void OnReleaseObject(InputValue value)
    {
        if (value.isPressed)
        {
            pickUp.ReleaseObject();
        }
    }
}
