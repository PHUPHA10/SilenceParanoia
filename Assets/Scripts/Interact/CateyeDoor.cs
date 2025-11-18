using UnityEngine;
using UnityEngine.Rendering;

public class CateyeDoor : MonoBehaviour, IInteractable
{
    [Header("Cameras")]
    [SerializeField] private Camera playerCamera;   // ????????? (Main Camera)
    [SerializeField] private Camera cateyeCamera;   // ??????????

    [Header("Optional: Volume ????? (??? Global)")]
    [SerializeField] private Volume cateyeVolume;   // ?????????????????????

    [Header("Disable Player Controls While Looking")]
    [SerializeField] private MonoBehaviour[] componentsToDisable;
    // ???? FirstPersonController, StarterAssetsInputs ???

    private bool isLooking = false;

    // ?????????? PlayerInteract ???????????
    public string Prompt => isLooking ? "stop looking" : "look through cateye";

    void Start()
    {
        if (cateyeCamera != null)
            cateyeCamera.enabled = false;

        if (cateyeVolume != null)
            cateyeVolume.enabled = false;
    }

    // ????????????? E ??????????? ?????????????? IInteractable ??????
    public void Interact() { }

    // ??? PlayerInteract ?????????????????? Q
    public void ToggleCateye()
    {
        if (!isLooking) EnterCateye();
        else ExitCateye();
    }

    private void EnterCateye()
    {
        isLooking = true;

        // ?????????
        if (playerCamera != null) playerCamera.enabled = false;
        if (cateyeCamera != null) cateyeCamera.enabled = true;

        // ???? Volume ????????????? (?????)
        if (cateyeVolume != null) cateyeVolume.enabled = true;

        // ??????????/?????????
        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ExitCateye()
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
