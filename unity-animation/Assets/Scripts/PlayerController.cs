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
        MovePlayer();
        HandleJump();
        CheckFall();
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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator?.SetBool("isJumping", true);
        }
    }

    void CheckFall()
    {
        if (transform.position.y < fallThreshold)
            Respawn();
    }

    void Respawn()
    {
        transform.position = startPosition + Vector3.up * 5;
        rb.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player landed!");

            if (animator != null)
            {
                animator.SetBool("isJumping", false);
                bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
                animator.SetBool("isRunning", isMoving);
            }
        }
    }
}
