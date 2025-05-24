using UnityEngine;

public class MouseLookScript : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 300f;
    public float yRotationSpeed = 0.1f;
    public float xCameraSpeed = 0.1f;
    public float topAngleView = 60f;
	[Header("Sensitivity Presets")]
public float mouseSensitvity_aiming = 50f;
public float mouseSensitvity_notAiming = 300f;

    public float bottomAngleView = -45f;
    public bool showFps = true;

    public float rotationYVelocity, cameraXVelocity;
    public float wantedYRotation, currentYRotation;
    public float wantedCameraXRotation, currentCameraXRotation;
    public float zRotation;

    public float deltaTime = 0.0f;
    public Transform myCamera;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        myCamera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
        if (myCamera == null)
            Debug.LogError("MainCamera not found! Assign the MainCamera tag to your camera.");
    }

    void Update()
    {
        MouseInputMovement();

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void FixedUpdate()
    {
        ApplyMouseMovement();
    }

    void MouseInputMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        wantedYRotation += mouseX;
        wantedCameraXRotation -= mouseY;
        wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);
    }

    void ApplyMouseMovement()
    {
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
        if (myCamera != null)
            myCamera.localRotation = Quaternion.Euler(currentCameraXRotation, 0f, zRotation);
    }

    void OnGUI()
    {
        if (showFps)
            ShowFPS();
    }

    void ShowFPS()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = h * 2 / 100,
            normal = { textColor = Color.white }
        };

        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
