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

    [Header("Zoom Settings")]
    public float minZoom = 3f;   // Minimum zoom distance
    public float maxZoom = 10f;  // Maximum zoom distance
    public float zoomSpeed = 2f; // Speed of zooming
    private float currentZoom;   // Holds the current zoom level

    [Header("Smoothing Settings")]
    public float smoothSpeed = 0.1f; // Smoothing factor for camera movement

    private Vector3 desiredPosition; // Target position for smoothing

    void Start()
    {
        currentZoom = Mathf.Abs(offset.z); // Set initial zoom distance
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
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

        // Camera Rotation
        if (!requireRightClick || Input.GetMouseButton(1)) // Free rotation OR right-click hold
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -30f, 60f); // Prevent extreme up/down rotation
        }

        // Calculate desired position and apply smoothing
        desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Smooth rotation
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed);
    }
}
