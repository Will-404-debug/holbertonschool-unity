using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;

    private Vector3 startPosition; 
    private float fallThreshold = -10f; 

    public Transform cameraTransform; 
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Force re-assign the Animator at runtime
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(); // Try to find it in child objects
        }

        if (animator == null)
        {
            Debug.LogError("❌ ERROR: Animator component is STILL missing at runtime! Check if the Player GameObject has an Animator.");
        }
        else
        {
            Debug.Log("✅ Animator found and assigned successfully.");
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("❌ ERROR: No Main Camera found!");
            }
        }

        startPosition = transform.position; // Store the starting position
    }

    void Update()
    {
        MovePlayer();
        HandleJump();
        CheckFall();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

            transform.position += moveDirection * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
    }

    private void CheckFall()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player landed! isGrounded set to true.");

            if (animator != null)
            {
                animator.SetBool("isJumping", false);

                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    animator.SetBool("isRunning", true);
                }
                else
                {
                    animator.SetBool("isRunning", false);
                }
            }
            else
            {
                Debug.LogError("❌ ERROR: Animator is null in OnCollisionEnter! Check if the Player has an Animator component.");
            }
        }
    }

    void Respawn()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y + 5, startPosition.z);
        rb.velocity = Vector3.zero; 
    }
}
