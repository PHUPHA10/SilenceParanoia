using UnityEngine;

public class MicPermissionManager : MonoBehaviour
{
    public static bool AllowMic = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("MicAllowed"))
        {
            AllowMic = PlayerPrefs.GetInt("MicAllowed") == 1;
        }
    }
}