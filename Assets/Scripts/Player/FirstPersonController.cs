using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump parameters")]
    [SerializeField] private float jumpForce = 1.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private Animator animator;

    [Header("Momentum Parameters")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;

    private Vector3 currentMovement;
    private float verticalRotation;
    private float currentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimations();
    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    private void HandleJumping()
    {
        currentMovement.y = -0.5f;
        if (playerInputHandler.JumpTriggered)
        {
            currentMovement.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            Vector3 worldDirection = CalculateWorldDirection();

            Vector3 targetHorizontalVelocity = worldDirection * currentSpeed;
            Vector3 currentHorizontal = new Vector3(currentMovement.x, 0f, currentMovement.z);

            //Frenando o acelerando?
            float momentumRate = worldDirection.magnitude > 0.1f ? acceleration : deceleration;

            // Interpolacion aka momentum propiamente dicho
            Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetHorizontalVelocity, momentumRate * Time.deltaTime);

            currentMovement.x = newHorizontal.x;
            currentMovement.z = newHorizontal.z;

            // pendientes/rampas
            Vector3 adjustedVelocity = AdjustVelocityToSlope(newHorizontal);

            HandleJumping();

            Vector3 moveVelocity = new Vector3(adjustedVelocity.x, currentMovement.y, adjustedVelocity.z);

            if (!playerInputHandler.JumpTriggered && adjustedVelocity.y < 0) // Fixeo rampas
            {
                moveVelocity.y = adjustedVelocity.y - 2.0f;
            }

            characterController.Move(moveVelocity * Time.deltaTime);
        }
        else // Momentum en el aire aka no se puede cambiar de dirección una vez en el aire
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }

    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity) // fix para que no salte cuando baja en una pendiente
    {
        Vector3 rayOrigin = transform.position + characterController.center;
        float rayLength = (characterController.height / 2f) + 0.5f;

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayLength))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    private void UpdateAnimations()
    {
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
        float currentHorizontalSpeed = horizontalVelocity.magnitude;

        animator.SetFloat("Speed", currentHorizontalSpeed);
        animator.SetBool("IsGrounded", characterController.isGrounded);
        animator.SetFloat("VerticalVelocity", characterController.velocity.y);

    }
}