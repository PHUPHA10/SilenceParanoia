using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoseManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer loseVideoPlayer;

    [Header("Video Audio")]
    public AudioSource videoAudioSource;   // 🔊 ควบคุมเสียงวิดีโอ
    [Range(0f, 1f)]
    public float videoVolume = 0.6f;        // ปรับความดังได้จาก Inspector

    [Header("Disable On Lose")]
    public Canvas[] canvasesToDisable;
    public GameObject[] gameObjectsToDisable;

    [Header("Scene")]
    public string nextSceneName = "Game0";

    bool isPlaying = false;

    void Start()
    {
        if (loseVideoPlayer != null)
        {
            loseVideoPlayer.loopPointReached += OnVideoEnd;
            loseVideoPlayer.gameObject.SetActive(false);

            // 🔊 บังคับใช้ AudioSource
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

        // ปิด QTE
        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.ForceStopQTE();

        // ปิด UI / ระบบเกม (ห้ามปิด object ที่มี VideoPlayer / AudioSource)
        foreach (var c in canvasesToDisable)
            if (c != null) c.enabled = false;

        foreach (var go in gameObjectsToDisable)
            if (go != null && go != loseVideoPlayer.gameObject)
                go.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        // 🎥 ใช้กล้องที่ active อยู่จริง (หลังเด้งจากที่ซ่อน)
        Camera activeCam = GetActiveCamera();
        if (activeCam != null && loseVideoPlayer.renderMode == VideoRenderMode.CameraNearPlane)
        {
            loseVideoPlayer.targetCamera = activeCam;
        }

        // 🔊 ตั้ง volume ก่อนเล่น
        if (videoAudioSource != null)
        {
            videoAudioSource.volume = videoVolume;
            videoAudioSource.enabled = true;
        }

        // 🎬 เล่นวิดีโอ
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
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
