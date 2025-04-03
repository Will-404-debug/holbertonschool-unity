using UnityEngine;
using System.Collections;

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
    private bool isLandingTriggered = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 1.2f;

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
        HandleFalling();

        Debug.Log($"isGrounded: {isGrounded}, velocityY: {rb.velocity.y}");
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
        float checkDistance = groundCheckDistance + 0.2f;

        isGrounded = Physics.SphereCast(rayOrigin, 0.2f, Vector3.down, out RaycastHit hit, checkDistance, groundLayer);

        Debug.DrawRay(rayOrigin, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);

        if (!wasGroundedLastFrame && isGrounded)
        {
            Debug.Log($"✅ Player LANDED on: {hit.collider.gameObject.name}");

            float distance = Vector3.Distance(transform.position, startPosition);

            if (animator.GetBool("isFalling") && distance < 1f)
            {
                Debug.Log("💥 Coroutine: Triggering Falling Flat Impact");
                StartCoroutine(PlayLandingImpact());
            }

            animator?.SetBool("isJumping", false);
            animator?.SetBool("isFalling", false);

            bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
            animator?.SetBool("isRunning", isMoving);
            animator?.SetBool("isIdle", !isMoving);
        }
    }

    void HandleFalling()
    {
        if (animator == null) return;

        if (!isGrounded && rb.velocity.y < -10f)
        {
            if (!animator.GetBool("isFalling"))
                Debug.Log("🔻 Falling...");

            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
        }
        else if (isGrounded)
        {
            animator.SetBool("isFalling", false);

            bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
            animator.SetBool("isRunning", isMoving);
            animator.SetBool("isIdle", !isMoving);
        }
        else if (!isGrounded && rb.velocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
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

    // ✅ Coroutine for landing impact
    IEnumerator PlayLandingImpact()
    {
        if (isLandingTriggered) yield break;

        isLandingTriggered = true;

        yield return new WaitForEndOfFrame(); // wait one frame

        animator.SetTrigger("hasLanded");

        yield return new WaitForSeconds(1.0f); // adjust duration to match your animation length

        isLandingTriggered = false;
    }
}
