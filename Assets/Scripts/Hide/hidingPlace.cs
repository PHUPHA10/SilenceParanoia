using UnityEngine;
using UnityEngine.Rendering;

public class hidingPlace : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;
    public Camera hideCamera;

    [Header("Player Model")]
    public GameObject playerModel;   // ? ????????????????????? (Mesh/Arms)
    public RotateLeftRightLimited hideviews;
    [Header("Player Control ()")]
    public MonoBehaviour[] componentsToDisable;

    private bool isHiding = false;

    public string Prompt =>
        isHiding ? " Exit "
                 : " Hide ";

    void Start()
    {
        if (hideCamera != null)
            hideCamera.enabled = false;
    }

    public void Interact()
    {
        var playerData = FindAnyObjectByType<PlayerData>();
        playerData.isHiding = true;

        hideviews.enabled = true;
        playerData.playerCamera = playerCamera;
        playerData.hidingCam = hideCamera;

        if (playerData.playerCamera != null) playerData.playerCamera.enabled = false;
        if (playerData.hidingCam != null) playerData.hidingCam.enabled = true;

        playerData.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        // Refresh prompt
        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null)
            playerInteract.RefreshPrompt(this);

    }
}
