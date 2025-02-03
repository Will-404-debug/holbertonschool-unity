using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the Player
    public Vector3 offset = new Vector3(0, 2.5f, -6.25f); // Camera offset from Player

    public float rotationSpeed = 3.0f; // Mouse sensitivity for rotation
    public bool requireRightClick = false; // If true, player must hold right-click to rotate camera

    private float pitch = 0f; // Up/down rotation (X-axis)
    private float yaw = 0f;   // Left/right rotation (Y-axis)

    void Start()
    {
        // Ensure camera starts at the correct position
        transform.position = player.position + offset;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Follow the player with a fixed offset
        transform.position = player.position + offset;

        // Mouse rotation logic
        if (!requireRightClick || Input.GetMouseButton(1)) // If not requiring right-click, or right-click is held
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -30f, 60f); // Prevent extreme up/down rotation

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
