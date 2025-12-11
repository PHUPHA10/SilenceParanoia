using UnityEngine;
using UnityEngine.Rendering;

public class hidingPlace : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;
    public Camera hideCamera;

    [Header("Player Model")]
    public GameObject playerModel;              // ??????? (mesh/arms ????????????)
    public RotateLeftRightLimited hideviews;    // ???????????????-??????????

    [Header("Player Control ()")]
    public MonoBehaviour[] componentsToDisable; // FirstPersonController, StarterAssetsInputs, PlayerInteract ???

    private bool isHiding = false;
    public bool IsHiding => isHiding;

    public string Prompt =>
        isHiding ? " Exit "
                 : " Hide ";

    void Start()
    {
        if (hideCamera != null)
            hideCamera.enabled = false;

        if (hideviews != null)
            hideviews.enabled = false;
    }

    public void Interact()
    {
        var playerData = FindAnyObjectByType<PlayerData>();
        if (playerData == null)
        {
            Debug.LogWarning("????? PlayerData ?????");
            return;
        }

        // ? ???????????????? + QTE ??????????? ? ???????
        if (isHiding && HidingQTEManager.Instance != null && HidingQTEManager.Instance.IsQteActive)
        {
            Debug.Log("???????????????????????? QTE");
            return;
        }

        if (!isHiding)
        {
            EnterHide(playerData);
        }
        else
        {
            ExitHide(playerData);
        }

        // Refresh prompt
        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null)
            playerInteract.RefreshPrompt(this);
    }

    // ??????????
    private void EnterHide(PlayerData playerData)
    {

        isHiding = true;
        playerData.isHiding = true;

        // ??? PlayerData ?????????????????? (????????????????)
        playerData.playerCamera = playerCamera;
        playerData.hidingCam = hideCamera;

        // ?????????
        if (playerCamera != null) playerCamera.enabled = false;
        if (hideCamera != null) hideCamera.enabled = true;

        // ??????????????? (????? model)
        if (playerModel != null)
            playerModel.SetActive(false);

        // ?????????????????????????????
        if (hideviews != null)
            hideviews.enabled = true;

        // ???????????????? ?
        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = false;
        }

        // ? ????????? playerData.gameObject ??????????????????????????????????????
        // playerData.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ?????????????
    private void ExitHide(PlayerData playerData)
    {
        Debug.Log("Exit Hiding");
        isHiding = false;
        playerData.isHiding = false;

        if (hideCamera != null) hideCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;

        if (playerModel != null)
            playerModel.SetActive(true);

        if (hideviews != null)
            hideviews.enabled = false;

        if (componentsToDisable != null)
        {
            foreach (var c in componentsToDisable)
                if (c != null) c.enabled = true;
        }

        playerData.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
