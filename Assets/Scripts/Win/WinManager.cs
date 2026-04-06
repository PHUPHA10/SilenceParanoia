using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class HideWinManager : MonoBehaviour
{
    [Header("Win Condition")]
    public float hideToWinTime = 60f;

    [Header("Video")]
    public VideoPlayer winVideo;
    public AudioSource videoAudio;

    [Header("Shutdown On Win")]
    public Canvas[] canvasesToDisable;
    public GameObject[] gameObjectsToDisable;

    [Header("Scene")]
    public string nextSceneName = "Menu";

    float hideTimer = 0f;
    bool isHiding = false;
    bool hasWon = false;

    void Start()
    {
        if (winVideo != null)
        {
            winVideo.loopPointReached += OnVideoEnd;
        }
    }

    void Update()
    {
        if (hasWon || !isHiding) return;

        hideTimer += Time.deltaTime;

        if (hideTimer >= hideToWinTime)
        {
            Win();
        }
    }


    public void OnPlayerEnterHide()
    {
        if (hasWon) return;

        hideTimer = 0f;
        isHiding = true;
    }

    public void OnPlayerExitHide()
    {
        if (hasWon) return; 

        isHiding = false;
        hideTimer = 0f;
    }

    Camera GetActiveCamera()
    {
        Camera[] cams = Camera.allCameras;
        foreach (var cam in cams)
        {
            if (cam.isActiveAndEnabled)
                return cam;
        }
        return Camera.main;
    }


    void Win()
    {
        if (hasWon) return;

        hasWon = true;
        isHiding = false;

        // รีเซ็ต QTE (ของเดิม)
        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.ForceStopQTE();

        // 🔥 ตรงนี้คือหัวใจ
        if (winVideo.renderMode == VideoRenderMode.CameraNearPlane)
        {
            winVideo.targetCamera = GetActiveCamera();
        }

        foreach (var c in canvasesToDisable)
            if (c != null) c.enabled = false;

        foreach (var go in gameObjectsToDisable)
            if (go != null) go.SetActive(false);

        if (winVideo != null)
        {
            winVideo.gameObject.SetActive(true);
            winVideo.Play();
        }
    }



    void OnVideoEnd(VideoPlayer vp)
    {
        PlayerPrefs.SetInt("GameCompleted", 1);
        PlayerPrefs.Save();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (videoAudio != null)
            videoAudio.Stop();

        SceneManager.LoadScene("MainMenu");
    }
}
