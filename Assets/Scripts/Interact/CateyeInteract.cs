using UnityEngine;

public class CateyeInteractInteract : MonoBehaviour, IInteractable
{
    public Camera MainCamera;
    public Camera CatEyeCamera;

    public string Prompt => "Look through CatEye";

    bool isLooking;

    void Start()
    {
        CatEyeCamera.enabled = false;
    }

    public void Interact()
    {
        if (!isLooking) EnterCateye();
        else ExitCateye();
    }

    void EnterCateye()
    {
        isLooking = true;
        MainCamera.enabled = false;
        CatEyeCamera.enabled = true;

        // ล็อกการหมุนกล้องผู้เล่น
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitCateye()
    {
        isLooking = false;
        CatEyeCamera.enabled = false;
        MainCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
