using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the Player
    public Vector3 offset = new Vector3(0, 2.5f, -6.25f); // Default Camera offset

    [Header("Camera Rotation Settings")]
    public float rotationSpeed = 3.0f;  // Mouse sensitivity
    public bool requireRightClick = false; // Hold right-click to rotate camera

    private float pitch = 0f; // Up/down rotation (X-axis)
    private float yaw = 0f;   // Left/right rotation (Y-axis)
    private float lastMouseMovementTime = 0f; // Track last mouse movement
    private float lastPlayerMovementTime = 0f; // Track last player movement

    [Header("Zoom Settings")]
    public float minZoom = 3f;   // Minimum zoom distance
    public float maxZoom = 10f;  // Maximum zoom distance
    public float zoomSpeed = 2f; // Speed of zooming
    private float currentZoom;   // Holds the current zoom level

    [Header("Smoothing Settings")]
    public float smoothSpeed = 0.1f; // Smoothing factor for camera movement

    [Header("Camera Collision Detection")]
    public LayerMask collisionLayers; // Layers to detect camera collision
    public float cameraCollisionRadius = 0.2f; // Radius for collision detection

    [Header("Auto Orbit Settings")]
    public bool enableAutoOrbit = true; // Toggle auto orbit feature
    public float idleTime = 5f; // Time before auto orbit activates
    public float autoOrbitSpeed = 10f; // Rotation speed when orbiting

    private Vector3 desiredPosition; // Target position for smoothing
    private bool isIdle = false; // Track if camera is in idle mode

    private Rigidbody playerRb; // Reference to player's Rigidbody

    void Start()
    {
        currentZoom = Mathf.Abs(offset.z); // Set initial zoom distance
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // Get the player's Rigidbody for movement detection
        playerRb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Handle Zoom (Mouse Scroll Wheel)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Update offset based on zoom
        offset = new Vector3(offset.x, offset.y, -currentZoom);

        // Detect Player Movement
        if (playerRb.velocity.magnitude > 0.1f) // Check if player is moving
        {
            lastPlayerMovementTime = Time.time; // Reset idle timer
            isIdle = false;
        }

        // Camera Rotation Logic
        bool isRotating = false;
        if (!requireRightClick || Input.GetMouseButton(1)) // Free rotation OR right-click hold
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            if (mouseX != 0 || mouseY != 0)
            {
                yaw += mouseX;
                pitch -= mouseY;
                lastMouseMovementTime = Time.time; // Reset idle timer
                isIdle = false;
                isRotating = true;
            }

            pitch = Mathf.Clamp(pitch, -30f, 60f); // Prevent extreme up/down rotation
        }

        // Auto Orbit Logic (if player is idle)
        if (enableAutoOrbit && !isRotating)
        {
            // Only activate idle mode if the player and mouse are idle
            if (Time.time - lastMouseMovementTime > idleTime && Time.time - lastPlayerMovementTime > idleTime)
            {
                isIdle = true;
            }

            // If idle, slowly orbit around the player
            if (isIdle)
            {
                yaw += autoOrbitSpeed * Time.deltaTime;
            }
        }

        // Camera Collision Detection
        Vector3 targetPosition = player.position + offset;
        RaycastHit hit;

        if (Physics.SphereCast(player.position, cameraCollisionRadius, offset.normalized, out hit, currentZoom, collisionLayers))
        {
            targetPosition = hit.point + hit.normal * cameraCollisionRadius;
        }

        // Smoothly move and rotate camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0f), smoothSpeed);

        // Camera Reset (Press "R" key)
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCamera();
        }
    }

    // Function to Reset Camera Position
    void ResetCamera()
    {
        yaw = player.eulerAngles.y; // Reset yaw to player's direction
        pitch = 10f; // Slight downward angle
        isIdle = false;
        lastMouseMovementTime = Time.time; // Reset idle timer
        lastPlayerMovementTime = Time.time;
    }
}
