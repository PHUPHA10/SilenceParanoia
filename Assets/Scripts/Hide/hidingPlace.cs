using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class hidingPlace : MonoBehaviour, IInteractable
{
    [Header("Camera Switching")]
    public Camera playerCamera;
    public Camera hideCamera;

    [Header("Player Model")]
    public GameObject playerModel;
    public RotateLeftRightLimited hideviews;

    [Header("Player Control")]
    public MonoBehaviour[] componentsToDisable;

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

    [Header("Enemy Spawn")]
    public EnemySpawnerHide enemySpawner;
    private Transform hideTransform;

    [Header("Lose Video")]
    public VideoClip loseVideoClip;

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

        if (hideCamera != null)
            hideCamera.enabled = false;

        if (hideviews != null)
            hideviews.enabled = false;
    }

    public void Interact()
    {
        if (isHiding) return;
        if (questTimer == null || !questTimer.IsQuestRunning) return;

        EnterHide();
    }

    private void EnterHide()
    {
        isHiding = true;
        hideTransform = transform;

        if (questTimer != null)
            questTimer.OnPlayerHide();

        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.RegisterHideSpot(this);

        if (playerCamera != null) playerCamera.enabled = false;
        if (hideCamera != null) hideCamera.enabled = true;

        if (playerModel != null)
            playerModel.SetActive(false);

        if (hideviews != null)
            hideviews.enabled = true;

        foreach (var c in componentsToDisable)
            if (c != null) c.enabled = false;

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

    public void StartFootstepAfterQuest()
    {
        if (!isHiding) return;

        if (footstepSource != null && footstepClip != null && footstepRoutine == null)
        {
            footstepRoutine = StartCoroutine(FootstepSequence());
        }
    }

    public void ForceExitHide()
    {
        if (!isHiding) return;

        isHiding = false;

        if (questTimer != null)
            questTimer.OnPlayerExitHide();

        if (HidingQTEManager.Instance != null)
            HidingQTEManager.Instance.UnregisterHideSpot(this);

        if (hideCamera != null) hideCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;

        if (playerModel != null)
            playerModel.SetActive(true);

        if (hideviews != null)
            hideviews.enabled = false;

        foreach (var c in componentsToDisable)
            if (c != null) c.enabled = true;

        if (heartbeatSource != null)
            heartbeatSource.Stop();

        PhoneChat.UnlockPhone();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hideWinManager != null)
            hideWinManager.OnPlayerExitHide();

        FindObjectOfType<LoseManager>()
            ?.PlayLoseVideo(loseVideoClip);

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

    }


    IEnumerator FootstepSequence()
    {
        footstepSource.clip = footstepClip;
        footstepSource.loop = false;

        while (isHiding)
        {
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

}
