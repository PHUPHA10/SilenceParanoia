using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoseManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer loseVideoPlayer;

    [Header("Video Audio")]
    public AudioSource videoAudioSource;
    [Range(0f, 1f)]
    public float videoVolume = 0.6f;

    [Header("Disable On Lose")]
    public Canvas[] canvasesToDisable;
    public GameObject[] gameObjectsToDisable;

    [Header("Scene")]
    public string nextSceneName = "MainMenu";

    bool isPlaying = false;

    void Start()
    {
        if (loseVideoPlayer != null)
        {
            loseVideoPlayer.loopPointReached += OnVideoEnd;
            loseVideoPlayer.gameObject.SetActive(false);

            loseVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

            if (videoAudioSource != null)
            {
                loseVideoPlayer.SetTargetAudioSource(0, videoAudioSource);
                videoAudioSource.playOnAwake = false;
                videoAudioSource.volume = videoVolume;
            }
        }
    }

    public void PlayLoseVideo(VideoClip clip)
    {
        if (isPlaying) return;
        if (clip == null) return;

        isPlaying = true;

        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.ForceStopQTE();

        foreach (var c in canvasesToDisable)
            if (c != null) c.enabled = false;

        foreach (var go in gameObjectsToDisable)
            if (go != null && go != loseVideoPlayer.gameObject)
                go.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        Camera activeCam = GetActiveCamera();
        if (activeCam != null && loseVideoPlayer.renderMode == VideoRenderMode.CameraNearPlane)
        {
            loseVideoPlayer.targetCamera = activeCam;
        }

        if (videoAudioSource != null)
        {
            videoAudioSource.volume = videoVolume;
            videoAudioSource.enabled = true;
        }

        loseVideoPlayer.gameObject.SetActive(true);
        loseVideoPlayer.clip = clip;
        loseVideoPlayer.Play();
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

    void OnVideoEnd(VideoPlayer vp)
    {

        PlayerPrefs.Save();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}