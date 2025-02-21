using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Camera & Player References")]
    public CinemachineFreeLook freeLookCamera;
    public Transform player;

    [Header("Rotation Settings")]
    public bool requireRightClick = true;
    public float rotationSpeed = 300f;
    public float playerRotationSpeed = 5f;
    public float rotationSpeedX = 0.1f;
    public float rotationSpeedY = 0.1f;
    public float cameraSmoothTime = 0.1f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoom = 30f;
    public float maxZoom = 60f;
    public float zoomSmoothTime = 0.1f;

    [Header("Auto Orbit Settings")]
    public bool enableAutoOrbit = true;
    public float idleTime = 5f;
    public float autoOrbitSpeed = 0.2f;

    private float lastInputTime;
    private bool isRotating = false;
    private float zoomVelocity = 0f;
    private Vector3 lastMousePosition;

    void Start()
    {
        if (freeLookCamera == null)
        {
            freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
            if (freeLookCamera == null)
            {
                Debug.LogError("❌ ERROR: No Cinemachine FreeLook Camera found!");
                return;
            }
        }

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
            else
            {
                Debug.LogError("❌ ERROR: No Player found!");
                return;
            }
        }

        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";
    }

    void Update()
    {
        if (freeLookCamera == null || player == null) return;

        HandleZoom();
        HandleRotation();
        HandleAutoOrbit();
        HandleCameraReset();
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float targetZoom = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView - scrollInput, minZoom, maxZoom);
        
        freeLookCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(
            freeLookCamera.m_Lens.FieldOfView, 
            targetZoom, 
            ref zoomVelocity, 
            zoomSmoothTime
        );
    }

    private void HandleRotation()
    {
        if (requireRightClick)
        {
            if (!Input.GetMouseButton(1))
            {
                isRotating = false;
                return;
            }
            isRotating = true;
            lastInputTime = Time.time;
        }

        float mouseX = Input.GetAxis("Mouse X") * (rotationSpeedX * 100) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeedY * Time.deltaTime;

        freeLookCamera.m_XAxis.Value += mouseX;
        freeLookCamera.m_YAxis.Value = Mathf.Clamp(freeLookCamera.m_YAxis.Value + mouseY, 0f, 1f);

        RotatePlayerToCamera();
    }

    private void RotatePlayerToCamera()
    {
        if (player == null || freeLookCamera == null) return;

        Vector3 cameraForward = freeLookCamera.transform.forward;
        cameraForward.y = 0;

        if (cameraForward.magnitude <= 0.1f) return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        if (Quaternion.Angle(player.rotation, targetRotation) > 1f)
        {
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * playerRotationSpeed);
        }
    }

    private void HandleAutoOrbit()
    {
        if (!enableAutoOrbit || isRotating || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            return;
        }

        if (Time.time - lastInputTime > idleTime)
        {
            freeLookCamera.m_XAxis.Value += autoOrbitSpeed;
        }
    }

    private void HandleCameraReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            freeLookCamera.m_XAxis.Value = 0;
            freeLookCamera.m_YAxis.Value = 0.5f;
            RotatePlayerToCamera();
        }
    }
}
