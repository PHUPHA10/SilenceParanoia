using StarterAssets;
using UnityEngine;

public class ChatOverlayController : MonoBehaviour
{
    public GameObject chatRoot;
    public KeyCode toggleKey = KeyCode.M;

    public static bool IsChatOpen { get; private set; }

    PlayerInteract playerInteract;
    CateyeDoor cateyeDoor;
    FlashlightController flashlightController;
    HotbarInput hotBar;
    FirstPersonController firstPersonController;

    void Start()
    {
        if (chatRoot != null)
            chatRoot.SetActive(false);

        IsChatOpen = false;

        playerInteract = FindObjectOfType<PlayerInteract>();
        cateyeDoor = FindObjectOfType<CateyeDoor>();
        flashlightController = FindObjectOfType<FlashlightController>();
        hotBar = FindObjectOfType<HotbarInput>();
        firstPersonController = FindAnyObjectByType<FirstPersonController>();
    }

    void Update()
    {
        // ???????????????????
        bool uiOpen = chatRoot != null && chatRoot.activeSelf;
        IsChatOpen = uiOpen;   // sync state ????????????????????

        // ?? M ? toggle ?????????????? UI
        if (Input.GetKeyDown(toggleKey))
        {
            if (uiOpen) CloseChat();
            else OpenChat();
        }

        // ?? ESC ? ?????? "?????????????????? ?"
        if (uiOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC close chat");
            CloseChat();
        }
    }

    void OpenChat()
    {
        if (chatRoot != null)
            chatRoot.SetActive(true);

        IsChatOpen = true;

        if (playerInteract != null) playerInteract.enabled = false;
        if (cateyeDoor != null) cateyeDoor.enabled = false;
        if (flashlightController != null) flashlightController.enabled = false;
        if (hotBar != null) hotBar.enabled = false;
        if (firstPersonController != null) firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseChat()
    {
        if (chatRoot != null)
            chatRoot.SetActive(false);

        IsChatOpen = false;

        if (playerInteract != null) playerInteract.enabled = true;
        if (cateyeDoor != null) cateyeDoor.enabled = true;
        if (flashlightController != null) flashlightController.enabled = true;
        if (hotBar != null) hotBar.enabled = true;
        if (firstPersonController != null) firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ????????????????????????????? notification ????????????????????
    public void OpenFromExternal()
    {
        OpenChat();
    }
}
