using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2.5f, -6.25f);
    
    private Vector3 startPosition; // Store camera's initial position
    private Quaternion startRotation; // Store camera's initial rotation

    [Header("Camera Rotation Settings")]
    public float rotationSpeed = 3.0f;
    public bool requireRightClick = false;
    private float pitch = 0f;
    private float yaw = 0f;

    [Header("Zoom Settings")]
    public float minZoom = 3f;
    public float maxZoom = 10f;
    public float zoomSpeed = 2f;
    private float currentZoom;

    [Header("Smoothing Settings")]
    public float smoothSpeed = 0.1f;

    [Header("Camera Collision Detection")]
    public LayerMask collisionLayers;
    public float cameraCollisionRadius = 0.2f;

    [Header("Auto Orbit Settings")]
    public bool enableAutoOrbit = true;
    public float idleTime = 5f;
    public float autoOrbitSpeed = 10f;

    private Vector3 desiredPosition;
    private bool isIdle = false;
    private Rigidbody playerRb;
    private float lastPlayerMovementTime = 0f;
    private float lastMouseMovementTime = 0f;

    void Start()
    {
        currentZoom = Mathf.Abs(offset.z);
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        playerRb = player.GetComponent<Rigidbody>();

        startPosition = transform.position; // Save camera start position
        startRotation = transform.rotation; // Save camera start rotation
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
        if (playerRb.velocity.magnitude > 0.1f)
        {
            lastPlayerMovementTime = Time.time;
            isIdle = false;
        }

        // Camera Rotation
        bool isRotating = false;
        if (!requireRightClick || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            if (mouseX != 0 || mouseY != 0)
            {
                yaw += mouseX;
                pitch -= mouseY;
                lastMouseMovementTime = Time.time;
                isIdle = false;
                isRotating = true;
            }

            pitch = Mathf.Clamp(pitch, -30f, 60f);
        }

        // Auto Orbit if Idle
        if (enableAutoOrbit && !isRotating)
        {
            if (Time.time - lastMouseMovementTime > idleTime && Time.time - lastPlayerMovementTime > idleTime)
            {
                isIdle = true;
            }

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

    // Reset Camera Position when Player Respawns
    public void ResetCamera()
    {
        yaw = player.eulerAngles.y;
        pitch = 10f;
        isIdle = false;
        lastMouseMovementTime = Time.time;
        lastPlayerMovementTime = Time.time;

        // Move Camera to Start Position
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
