using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    void Start()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data != null)
        {
            transform.position = new Vector3(
                data.playerX,
                data.playerY,
                data.playerZ
            );
        }
    }
}