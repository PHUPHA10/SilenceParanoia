using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneChat : MonoBehaviour
{
    [Header("UI")]
    public GameObject phoneChatRoot;
    public GameObject pauseMenu;
    public static bool IsChatOpen { get; private set; }

    public static bool IsPhoneLocked = false;

    PlayerInteract playerInteract;
    CateyeDoor cateyeDoor;
    FlashlightController flashlightController;
    HotbarInput hotBar;
    FirstPersonController firstPersonController;

    [Header("Footstep Audio")]
    public AudioSource footstepAudio;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ❌ ถ้า Pause เปิดอยู่ → ปิดแชททันที และไม่ให้กด M
        if (pauseMenu != null && pauseMenu.activeInHierarchy)
        {
            if (IsChatOpen)
                CloseChat();

            return;
        }

        // 🔒 ถ้าโทรศัพท์โดนล็อก และยังไม่ได้เปิดอยู่ → ห้ามเปิด
        if (IsPhoneLocked && !IsChatOpen)
            return;

        // เปิด / ปิด Chat ด้วย M
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (IsChatOpen && ChatInputFocus.IsAnyInputFocused)
                return;

            if (IsChatOpen)
                CloseChat();
            else
                OpenChat();

            if (footstepAudio != null)
                footstepAudio.Stop();
        }

        // ESC ปิดแชท
        if (Keyboard.current.escapeKey.wasPressedThisFrame && IsChatOpen)
        {
            CloseChat();
        }
    }
    void OpenChat()
    {
        // ❌ กันเปิดตอน Pause
        if (PauseMenuManager.IsPaused)
            return;

        if (IsPhoneLocked)
            return;

        IsChatOpen = true;

        if (phoneChatRoot != null)
            phoneChatRoot.SetActive(true);

        SetGameplayEnabled(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (footstepAudio != null)
        {
            footstepAudio.Stop();
            footstepAudio.volume = 0f;
        }
    }

    public void CloseChat()
    {
        IsChatOpen = false;

        if (phoneChatRoot != null)
            phoneChatRoot.SetActive(false);

        SetGameplayEnabled(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (footstepAudio != null)
        {
            footstepAudio.volume = 0.285f;
        }
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

    public static void LockPhone()
    {
        IsPhoneLocked = true;

        if (IsChatOpen)
        {
            PhoneChat phone = FindObjectOfType<PhoneChat>();
            if (phone != null)
                phone.CloseChat();
        }
    }

    public static void UnlockPhone()
    {
        IsPhoneLocked = false;
    }
}