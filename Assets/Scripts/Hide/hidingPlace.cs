using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class hidingPlace : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;

    [Header("Timeline")]
    public PlayableDirector[] ShowTimelines;


    [Header("Player Model")]
    public GameObject playerModel;
    public RotateLeftRightLimited hideviews;

    [Header("Player Control")]
    public GameObject[] gameObjectsToDisable;

    [Header("Quest")]
    public QuestTimer questTimer;

    private bool isHiding = false;
    public bool IsHiding => isHiding;

    [Header("Heartbeat")]
    public AudioSource heartbeatSource;

    HideWinManager hideWinManager;

    [Header("Footstep Sound")]
    public AudioSource footstepSource;
    public AudioClip footstepClip;

    [Range(0f, 1f)] public float farVolume = 0.3f;
    [Range(0f, 1f)] public float mediumVolume = 0.59f;
    [Range(0f, 1f)] public float nearVolume = 0.84f;

    public float stepInterval = 1.2f;
    private Coroutine footstepRoutine;

    [Header("Gunshot Sound")]
    public AudioSource gunshotSource;
    public AudioClip gunshotClip;

    [Header("Enemy Spawn")]
    public EnemySpawnerHide enemySpawner;
    private Transform hideTransform;

    [Header("Lose Timeline")]
    public PlayableDirector losetimeline;
    [SerializeField] VideoClip failVideoClip;
    [SerializeField] VideoPlayer failVideoPlayer;

    public GameObject HideDialog;

    public string Prompt
    {
        get
        {
            if (isHiding) return "";

            if (questTimer == null || !questTimer.IsQuestRunning)
                return "ซ่อน (ปิดเบรกเกอร์ก่อน)";

            return "ซ่อน";
        }
    }

    void Start()
    {
        hideWinManager = FindObjectOfType<HideWinManager>();

        if (hideviews != null)
            hideviews.enabled = false;

        if (losetimeline != null)
            losetimeline.enabled = false;
    }

    public void Interact()
    {
        if (isHiding) return;

        if (questTimer == null || !questTimer.IsQuestRunning)
            return;

        EnterHide();
    }

    private void EnterHide()
    {
        isHiding = true;
        Debug.Log("IS HIDING TRUE");

        hideTransform = transform;

        if (questTimer != null)
            questTimer.OnPlayerHide();
        questTimer.enabled = false;

        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.RegisterHideSpot(this);

        if (playerCamera != null)
            playerCamera.enabled = false;

        foreach (PlayableDirector timeline in ShowTimelines)
        {
            if (timeline != null)
            {
                timeline.gameObject.SetActive(true);
                timeline.Play();
            }

        }

        if (losetimeline != null)
            losetimeline.gameObject.SetActive(false);
        if (losetimeline != null)
            losetimeline.enabled = false;

        if (playerModel != null)
            playerModel.SetActive(false);

        if (hideviews != null)
            hideviews.enabled = true;

        // 🔥 ปิด object ที่ลากใส่
        foreach (GameObject obj in gameObjectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        if (heartbeatSource != null)
        {
            heartbeatSource.volume = 0.6f;
            heartbeatSource.Play();
        }

        PhoneChat.LockPhone();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hideWinManager != null)
            hideWinManager.OnPlayerEnterHide();
    }
    public void ForceExitHide()
    {
        if (!isHiding) return;

        isHiding = false;

        if (questTimer != null)
            questTimer.OnPlayerExitHide();

        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.UnregisterHideSpot(this);

        if (HideDialog != null)
            HideDialog.SetActive(false);

        if (playerCamera != null)
            playerCamera.enabled = false;

        if (playerModel != null)
            playerModel.SetActive(false);

        if (hideviews != null)
            hideviews.enabled = false;

        if (heartbeatSource != null)
            heartbeatSource.Stop();

        PhoneChat.UnlockPhone();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hideWinManager != null)
            hideWinManager.OnPlayerExitHide();

        foreach (PlayableDirector timeline in ShowTimelines)
            if (timeline != null)
                timeline.gameObject.SetActive(true);
        if (losetimeline != null)
        {
            losetimeline.gameObject.SetActive(true);
            losetimeline.enabled = true;
        }

        if (footstepRoutine != null)
        {
            StopCoroutine(footstepRoutine);
            footstepRoutine = null;
        }

        if (footstepSource != null)
            footstepSource.Stop();

        if (enemySpawner != null && hideTransform != null)
        {
            enemySpawner.SpawnEnemyNearPlayer(hideTransform);
        }
        StartCoroutine(WaitTimelineThenLose());
    }
    IEnumerator WaitTimelineThenLose()
    {

        foreach (PlayableDirector timeline in ShowTimelines)
        {
            if (timeline != null)
            {
                timeline.Stop();
                timeline.gameObject.SetActive(false);
            }
        }

        if (losetimeline != null)
        {
            losetimeline.gameObject.SetActive(true);
            losetimeline.Play();
        }

        yield return new WaitWhile(() => losetimeline.state == PlayState.Playing);

        if (failVideoPlayer != null && failVideoClip != null)
        {
            bool videoFinished = false;
            if (losetimeline != null)
            {
                losetimeline.Stop();
                losetimeline.enabled = false;
            }
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            if (playerCamera != null)
            {
                playerCamera.enabled = true;
                playerCamera.gameObject.SetActive(true);
            }

            if (playerModel != null)
            {
                playerModel.SetActive(true);
            }

            PhoneChat.UnlockPhone();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            FirstPersonController controller =
                   FindObjectOfType<FirstPersonController>();

            if (controller != null)
            {
                controller.enabled = false;
            }

            failVideoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            failVideoPlayer.targetCamera = playerCamera;

            failVideoPlayer.gameObject.SetActive(true);
            failVideoPlayer.clip = failVideoClip;

            failVideoPlayer.loopPointReached += _ => videoFinished = true;
            failVideoPlayer.Play();

            if (hideWinManager != null)
                hideWinManager.OnPlayerExitHide();

            foreach (var go in gameObjectsToDisable)
            {
                if (go != null && go != failVideoPlayer.gameObject && go != playerCamera.gameObject)
                    go.SetActive(false);
            }
            yield return new WaitUntil(() => videoFinished);
            failVideoPlayer.gameObject.SetActive(false);

        }
        FindObjectOfType<LoseManager>()?.PlayLoseVideo(failVideoClip);
        {
            Debug.Log("หาแล้ว");
        }


    }

    public void StartFootstepAfterQuest()
    {
        if (!isHiding) return;

        if (footstepSource != null &&
            footstepClip != null &&
            footstepRoutine == null)
        {
            footstepRoutine = StartCoroutine(FootstepSequence());
        }
    }



    IEnumerator FootstepSequence()
    {
        footstepSource.clip = footstepClip;
        footstepSource.loop = false;

        while (isHiding)
        {
            if (Random.value < 0.3f)
                PlayGunshot();

            PlayFootstep(mediumVolume);
            yield return new WaitForSeconds(stepInterval);

            PlayFootstep(farVolume);
            yield return new WaitForSeconds(stepInterval + 0.3f);

            PlayFootstep(nearVolume);
            yield return new WaitForSeconds(stepInterval - 0.2f);

            PlayFootstep(mediumVolume);
            yield return new WaitForSeconds(stepInterval + 0.5f);
        }
    }

    void PlayFootstep(float volume)
    {
        if (footstepSource == null) return;

        footstepSource.volume = volume;
        footstepSource.PlayOneShot(footstepClip);
    }

    public void PlayGunshot()
    {
        if (gunshotSource != null && gunshotClip != null)
        {
            gunshotSource.pitch = Random.Range(0.9f, 1.1f);
            gunshotSource.PlayOneShot(gunshotClip);
        }
    }
}