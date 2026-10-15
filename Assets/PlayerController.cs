using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool faceMoveDirection;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 9f;
    [SerializeField] private float jumpForce = 5f;

    private Animator animator;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isRunning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // hold Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        UpdateAnimator();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * moveInput.y + right * moveInput.x;

        // use runSpeed when shift is held, moveSpeed otherwise
        float currentSpeed = isRunning ? runSpeed : moveSpeed;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = movement.x * currentSpeed;
        velocity.z = movement.z * currentSpeed;
        rb.linearVelocity = velocity;

        // turning — rotate character to face movement direction
        if (movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }

    void OnCollisionStay(Collision collision) { isGrounded = true; }
    void OnCollisionExit(Collision collision) { isGrounded = false; }

    void UpdateAnimator()
    {
        if (animator == null) return;

        float currentSpeed = new Vector3(moveInput.x, 0f, moveInput.y).magnitude;

        // when running multiply the speed value so the animator
        // crosses the run threshold — set your run transition at Speed > 0.6
        if (isRunning) currentSpeed *= 2f;

        animator.SetFloat("Speed", currentSpeed);
    }
}