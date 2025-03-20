using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;

    private Vector3 startPosition; // Store the player's starting position
    private float fallThreshold = -10f; // Y-position where the player is considered "falling"

    public Transform cameraTransform; // Reference to the main camera
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Assign the Animator component
        startPosition = transform.position; // Store the starting position

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Automatically find the main camera
        }
    }

    void Update()
    {
        MovePlayer();
        HandleJump();
        CheckFall();
    }

    private void MovePlayer()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create input movement vector
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Get the camera's forward and right vectors
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            // Flatten the vectors so movement is always on the ground plane
            camForward.y = 0;
            camRight.y = 0;

            // Normalize to ensure movement speed is consistent in all directions
            camForward.Normalize();
            camRight.Normalize();

            // Convert input direction to world space using the camera's orientation
            Vector3 moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

            // Move the player
            transform.position += moveDirection * speed * Time.deltaTime;

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void HandleJump()
    {
        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void CheckFall()
    {
        // Check if the player has fallen
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void Respawn()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y + 5, startPosition.z); // Respawn above start position
        rb.velocity = Vector3.zero; // Reset velocity to prevent falling again
    }
}
