using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlayerManager : MonoBehaviour
{

    public GameObject Player;
    public GameObject Hidingplace;
    private PlayerData playerData;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerData == null)
        {
            playerData = Player.GetComponent<PlayerData>();
        }

        if (playerData.isHiding && Input.GetKeyDown(KeyCode.E))
        {
            playerData.isHiding = false;

            if (playerData.hidingCam != null) playerData.hidingCam.enabled = false;
            if (playerData.playerCamera != null) playerData.playerCamera.enabled = true;

            Player.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
