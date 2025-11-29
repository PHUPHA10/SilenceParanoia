using UnityEngine;

public class ChatOverlayController : MonoBehaviour
{
    public GameObject chatRoot;
    public KeyCode toggleKey = KeyCode.M;

    public static bool IsChatOpen { get; private set; }

    // ??????????? ? ?????????????????????
    PlayerInteract playerInteract;
    CateyeDoor cateyeDoor;
    FlashlightController flashlightController;
    HotbarInput hotBar;

    void Start()
    {
        if (chatRoot != null)
            chatRoot.SetActive(false);

        // ? ????????????????????
        playerInteract = FindObjectOfType<PlayerInteract>();
        cateyeDoor = FindObjectOfType<CateyeDoor>();
        flashlightController = FindObjectOfType<FlashlightController>();
        hotBar = FindObjectOfType<HotbarInput>();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleChat();
        }
    }

    void ToggleChat()
    {
        IsChatOpen = !IsChatOpen;

        if (chatRoot != null)
            chatRoot.SetActive(IsChatOpen);

        // ? ????????????????????
        if (playerInteract != null) playerInteract.enabled = !IsChatOpen;
        if (cateyeDoor != null) cateyeDoor.enabled = !IsChatOpen;
        if (flashlightController != null) flashlightController.enabled = !IsChatOpen;
        if (hotBar != null) hotBar.enabled = !IsChatOpen;

        // ? ??????? FirstPersonController ???? StarterAssetsInputs
        // ?????????????????????? + ?????????
    }
}
