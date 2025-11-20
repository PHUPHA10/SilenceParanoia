using UnityEngine;
using UnityEngine.Rendering;

public class CateyeDoor : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;      // ????????? (MainCamera)
    public Camera cateyeCamera;      // ??????????

    [Header("Effects (Optional)")]
    public Volume cateyeVolume;      // Volume ???????? (????????????????????)

    [Header("Player Control (???????????????)")]
    public MonoBehaviour[] componentsToDisable;
    // ??? FirstPersonController, StarterAssetsInputs, ???? PlayerInteract ?????????????

    private bool isLooking = false;

    // ????????????????????????
    public string Prompt =>
        isLooking ? "stop checking outside "
                  : "check outside the room ";

    void Start()
    {
        if (cateyeCamera != null)
            cateyeCamera.enabled = false;

        if (cateyeVolume != null)
            cateyeVolume.enabled = false;
    }

    // ?????? E ???????? ?????????????? IInteractable ??????
    public void Interact() { }

    public void ToggleCateye()
    {
        if (!isLooking)
            Enter();
        else
            Exit();

        // ??? PlayerInteract ???????????????????????????????????
        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null)
        {
            playerInteract.RefreshPrompt(this);
        }
    }

    private void Enter()
    {
        isLooking = true;

        if (playerCamera != null) playerCamera.enabled = false;
        if (cateyeCamera != null) cateyeCamera.enabled = true;
        if (cateyeVolume != null) cateyeVolume.enabled = true;

        // ??????????/??????????????
        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Exit()
    {
        isLooking = false;

        if (cateyeCamera != null) cateyeCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;
        if (cateyeVolume != null) cateyeVolume.enabled = false;

        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
