using UnityEngine;
using UnityEngine.Rendering;

public class CateyeDoor : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;
    public Camera cateyeCamera;

    [Header("Effects")]
    public Volume cateyeVolume;

    [Header("Gameplay Lock")]
    FlashlightController flashlight;
    HotbarInput hotbar;
    MonoBehaviour firstPersonController;

    private bool isLooking = false;

    public string Prompt =>
        isLooking ? "เลิกส่องตาแมว" : "ส่องตาแมว";

    // ----------------------------
    // CATEYE LOOK SETTINGS
    // ----------------------------
    [Header("Cateye Look")]
    public float lookSpeed = 2f;
    public float maxYaw = 15f;   // ซ้ายขวา
    public float maxPitch = 10f; // ขึ้นลง

    float yaw;
    float pitch;
    Quaternion originalRotation;

    [Header("Footstep Audio")]
    public AudioSource footstepAudio;

    void Start()
    {
        if (cateyeCamera != null)
        {
            cateyeCamera.enabled = false;
            originalRotation = cateyeCamera.transform.localRotation;
        }

        if (cateyeVolume != null)
            cateyeVolume.enabled = false;

        flashlight = FindObjectOfType<FlashlightController>();
        hotbar = FindObjectOfType<HotbarInput>();
        firstPersonController = FindAnyObjectByType<StarterAssets.FirstPersonController>();
    }

    void Update()
    {
        if (!isLooking || cateyeCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        cateyeCamera.transform.localRotation =
            originalRotation * Quaternion.Euler(pitch, yaw, 0f);

        if (footstepAudio != null)
            footstepAudio.Stop();
    }

    public void Interact()
    {
        ToggleCateye();
    }

    void ToggleCateye()
    {
        if (!isLooking)
            Enter();
        else
            Exit();
    }

    void Enter()
    {
        isLooking = true;

        yaw = 0;
        pitch = 0;

        if (playerCamera != null) playerCamera.enabled = false;
        if (cateyeCamera != null) cateyeCamera.enabled = true;
        if (cateyeVolume != null) cateyeVolume.enabled = true;

        // 🔒 ปิด gameplay
        if (flashlight != null) flashlight.enabled = false;
        if (hotbar != null) hotbar.enabled = false;
        if (firstPersonController != null) firstPersonController.enabled = false;

        PhoneChat.LockPhone();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (footstepAudio != null)
            footstepAudio.Stop();
    }

    void Exit()
    {
        isLooking = false;

        if (cateyeCamera != null)
        {
            cateyeCamera.enabled = false;
            cateyeCamera.transform.localRotation = originalRotation;
        }

        if (playerCamera != null) playerCamera.enabled = true;
        if (cateyeVolume != null) cateyeVolume.enabled = false;

        // 🔓 เปิด gameplay กลับ
        if (flashlight != null) flashlight.enabled = true;
        if (hotbar != null) hotbar.enabled = true;
        if (firstPersonController != null) firstPersonController.enabled = true;

        PhoneChat.UnlockPhone();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
