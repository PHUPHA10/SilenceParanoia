using UnityEngine;

public class CarLookController : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 80f;

    [Header("Horizontal Limits")]
    public float leftLimit = -70f;
    public float rightLimit = 70f;

    [Header("Vertical Limits")]
    public float upLimit = 35f;
    public float downLimit = -35f;

    public bool lockLook = false;

    float yaw;
    float pitch;

    void Update()
    {
        if (PauseMenuManager.IsVideoPlaying || PhoneChat.IsChatOpen) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        yaw = Mathf.Clamp(yaw, leftLimit, rightLimit);
        pitch = Mathf.Clamp(pitch, downLimit, upLimit);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}