using UnityEngine;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    public Transform target; // персонаж (капсула)
    public float distance = 4f;
    public float height = 2f;
    public float rotationSpeed = 5f;

    private float currentX = 0f;
    private float currentY = 0f;
    public float yMin = -20f;
    public float yMax = 80f;

    void LateUpdate()
    {
        if (!target) return;

        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, yMin, yMax);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 dir = new Vector3(0, 0, -distance);
        Vector3 position = target.position + Vector3.up * height + rotation * dir;

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
