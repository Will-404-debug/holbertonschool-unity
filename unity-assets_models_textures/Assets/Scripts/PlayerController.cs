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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Store the starting position
    }

    void Update()
    {
        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create movement vector
        Vector3 movement = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;

        // Apply movement
        transform.Translate(movement, Space.Self);

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

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
