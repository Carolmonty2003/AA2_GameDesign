using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float gravityRotationSpeed = 8f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;

    [Header("Salto y gravedad actual")]
    public float gravity = -20f;
    public float jumpHeight = 1.2f;

    [Header("Valores normales")]
    public float normalGravity = -20f;
    public float normalJumpHeight = 1.2f;

    [Header("Valores luna / Puzzle 2")]
    public float moonGravity = -6f;
    public float moonJumpHeight = 2.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 fallVelocity;
    private bool isGrounded;
    private Vector3 currentGravityDir = Vector3.down;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        gravity = normalGravity;
        jumpHeight = normalJumpHeight;
    }

    public void SetGravityDirection(Vector3 newGravityDir, float newGravityForce)
    {
        if (newGravityDir == Vector3.zero) return;

        currentGravityDir = newGravityDir.normalized;
        gravity = newGravityForce;
    }

    public void SetMoonMode()
    {
        gravity = moonGravity;
        jumpHeight = moonJumpHeight;
    }

    public void SetNormalMode()
    {
        gravity = normalGravity;
        jumpHeight = normalJumpHeight;
    }

    void Update()
    {
        // Rotación suave
        Vector3 targetUp = -currentGravityDir;
        if (transform.up != targetUp)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, gravityRotationSpeed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (isGrounded)
        {
            if (Vector3.Dot(fallVelocity, currentGravityDir) > 0)
            {
                fallVelocity = currentGravityDir * 2f;
            }

            if (Input.GetButtonDown("Jump"))
            {
                fallVelocity = -currentGravityDir * Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        float currentGravityMagnitude = Mathf.Abs(gravity);
        fallVelocity += currentGravityDir * currentGravityMagnitude * Time.deltaTime;

        controller.Move(fallVelocity * Time.deltaTime);
    }
}