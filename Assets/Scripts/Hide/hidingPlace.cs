using UnityEngine;
using UnityEngine.Rendering;

public class hidingPlace : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;
    public Camera hideCamera;


    [Header("Player Control ()")]
    public MonoBehaviour[] componentsToDisable;
    // ??? FirstPersonController, StarterAssetsInputs, PlayerInteract 

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
        ToggleHide();
    }

    public void ToggleHide()
    {
        if (!isHiding)
            Enter2();
        else
            Exit2();

        //PlayerInteract 
        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null)
        {
            playerInteract.RefreshPrompt(this);
        }
    }

    private void Enter2()
    {
        isHiding = true;

        if (playerCamera != null) playerCamera.enabled = false;
        if (hideCamera != null) hideCamera.enabled = true;


        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Exit2()
    {
        isHiding = false;

        if (hideCamera != null) hideCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;

        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
