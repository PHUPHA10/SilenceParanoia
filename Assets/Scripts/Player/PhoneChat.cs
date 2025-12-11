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

        playerInteract = FindObjectOfType<PlayerInteract>();
        cateyeDoor = FindObjectOfType<CateyeDoor>();
        flashlightController = FindObjectOfType<FlashlightController>();
        hotBar = FindObjectOfType<HotbarInput>();
        firstPersonController = FindAnyObjectByType<FirstPersonController>();
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            
            if (IsChatOpen && ChatInputFocus.IsAnyInputFocused)
                return;

            ToggleChat();
        }
        
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ExitChat();
        }

    }

    void ToggleChat()
    {
        IsChatOpen = !IsChatOpen;

        if (phoneChatRoot != null)
            phoneChatRoot.SetActive(IsChatOpen);

        if (playerInteract != null) playerInteract.enabled = !IsChatOpen;
        if (cateyeDoor != null) cateyeDoor.enabled = !IsChatOpen;
        if (flashlightController != null) flashlightController.enabled = !IsChatOpen;
        if (hotBar != null) hotBar.enabled = !IsChatOpen;
        if (firstPersonController != null) firstPersonController.enabled = !IsChatOpen;
    }

    void ExitChat()
    {
        phoneChatRoot.SetActive(false);

        if (playerInteract != null) playerInteract.enabled = !IsChatOpen;
        if (cateyeDoor != null) cateyeDoor.enabled = !IsChatOpen;
        if (flashlightController != null) flashlightController.enabled = !IsChatOpen;
        if (hotBar != null) hotBar.enabled = !IsChatOpen;
        if (firstPersonController != null) firstPersonController.enabled = !IsChatOpen;
    }
}
