using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneChat : MonoBehaviour
{
    [Header("UI")]
    public GameObject phoneChatRoot;

    public static bool IsChatOpen { get; private set; }

    PlayerInteract playerInteract;
    CateyeDoor cateyeDoor;
    FlashlightController flashlightController;
    HotbarInput hotBar;
    FirstPersonController firstPersonController;

    void Start()
    {
        if (phoneChatRoot != null)
            phoneChatRoot.SetActive(false);

        IsChatOpen = false;

        playerInteract = FindObjectOfType<PlayerInteract>();
        cateyeDoor = FindObjectOfType<CateyeDoor>();
        flashlightController = FindObjectOfType<FlashlightController>();
        hotBar = FindObjectOfType<HotbarInput>();
        firstPersonController = FindAnyObjectByType<FirstPersonController>();

        // FPS mode ????????
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (IsChatOpen && ChatInputFocus.IsAnyInputFocused)
                return;

            if (IsChatOpen) CloseChat();
            else OpenChat();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && IsChatOpen)
        {
            CloseChat();
        }
    }

    void OpenChat()
    {
        IsChatOpen = true;

        if (phoneChatRoot != null)
            phoneChatRoot.SetActive(true);

        SetGameplayEnabled(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseChat()
    {
        IsChatOpen = false;

        if (phoneChatRoot != null)
            phoneChatRoot.SetActive(false);

        SetGameplayEnabled(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void SetGameplayEnabled(bool enabled)
    {
        if (playerInteract != null) playerInteract.enabled = enabled;
        if (cateyeDoor != null) cateyeDoor.enabled = enabled;
        if (flashlightController != null) flashlightController.enabled = enabled;
        if (hotBar != null) hotBar.enabled = enabled;
        if (firstPersonController != null) firstPersonController.enabled = enabled;
    }

    public void OpenFromNotification()
    {
        OpenChat();
    }
}
