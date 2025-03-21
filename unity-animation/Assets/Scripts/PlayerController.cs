using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Fall & Respawn")]
    private Vector3 startPosition;
    private float fallThreshold = -10f;

    [Header("References")]
    public Transform cameraTransform;
    private Rigidbody rb;
    private Animator animator;

    private bool isGrounded;
    private bool wasGroundedLastFrame;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError("❌ ERROR: Animator component is missing!");
        else
            Debug.Log("✅ Animator assigned.");

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
                Debug.LogError("❌ ERROR: No Main Camera found!");
        }

        startPosition = transform.position;
    }

    void Update()
    {
        CheckGroundStatus();
        MovePlayer();
        HandleJump();
        CheckFall();

        // Falling detection
        if (!isGrounded && rb.velocity.y < -0.1f)
        {
            animator?.SetBool("isFalling", true);
            animator?.SetBool("isJumping", false);
            animator?.SetBool("isRunning", false);
            animator?.SetBool("isIdle", false);
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(moveX, 0, moveZ).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

            transform.position += moveDir * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 10f);

            animator?.SetBool("isRunning", true);
        }
        else
        {
            animator?.SetBool("isRunning", false);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

            animator?.SetBool("isJumping", true);
            animator?.SetBool("isFalling", false);
        }
    }

    void CheckFall()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void CheckGroundStatus()
    {
        wasGroundedLastFrame = isGrounded;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float checkDistance = groundCheckDistance;

        Ray ray = new Ray(rayOrigin, Vector3.down);
        isGrounded = Physics.Raycast(ray, out RaycastHit hit, checkDistance, groundLayer);

        Debug.DrawRay(rayOrigin, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);

        if (!wasGroundedLastFrame && isGrounded)
        {
            Debug.Log("✅ Player LANDED on: " + hit.collider.gameObject.name);
            
            animator?.SetBool("isJumping", false);
            animator?.SetBool("isFalling", false);

            // Determine if the player is still moving
            bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
            animator?.SetBool("isRunning", isMoving);
            animator?.SetBool("isIdle", !isMoving);
        }
    }


    void Respawn()
    {
        transform.position = startPosition + Vector3.up * 5;
        rb.velocity = Vector3.zero;
        
        animator?.SetBool("isJumping", false);
        animator?.SetBool("isFalling", false);
        animator?.SetBool("isRunning", false);
        animator?.SetBool("isIdle", true);
    }
}
