using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using StarterAssets;



public class QuestTimer : MonoBehaviour
{
    [Header("UI")]
    public Canvas questCanvas;
    public GameObject questTextGroup;
    public TMP_Text timerText;

    [Header("Quest")]
    public float questTime = 25f;

    [Header("Flow")]
    public GameObject findQuestGroup;


    public UnityEvent OnQuestTimeEnd;

    [Header("Enemy")]
    public EnemySpawner enemySpawner;

    [Header("Quest Audio")]
    public AudioSource questAudio;
    public AudioClip questStartSound;

    [Header("Door Audio")]
    public AudioSource DoorAudio;
    public AudioClip knockSoftClip;
    public AudioClip knockLoudClip;
    public AudioClip kickDoorClip;

    [Header("Knock Timing")]
    public float minKnockDelay = 2.15f;
    public float maxKnockDelay = 4.85f;
    [Range(0f, 1f)]
    public float loudKnockChance = 0.35f;

    private float timer;
    private bool isRunning = false;
    private Coroutine knockRoutine;

    [Header("Door (Open After Quest)")]
    public Doorquest doorToOpen;

    [Header("Fail Video")]
    public UnityEngine.Video.VideoPlayer failVideo;
    public GameObject[] gameObjectsToDisable;


    public bool IsQuestRunning => isRunning;

    public void StartQuest()
    {
        timer = questTime;
        isRunning = true;

        if (findQuestGroup != null)
            findQuestGroup.SetActive(false);

        if (questCanvas != null)
        {
            questCanvas.gameObject.SetActive(true);
            questCanvas.enabled = true;
        }

        if (questTextGroup != null)
        {
            questTextGroup.SetActive(true);
            foreach (Transform t in questTextGroup.transform)
                t.gameObject.SetActive(true);
        }

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        if (questAudio != null && questStartSound != null)
            questAudio.PlayOneShot(questStartSound);

        if (DoorAudio != null &&
            (knockSoftClip != null || knockLoudClip != null))
        {
            knockRoutine = StartCoroutine(KnockLoop());
        }


        if (enemySpawner != null)
            enemySpawner.SpawnEnemy();
    }

    void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;

        if (timerText != null)
            timerText.text = $"เวลาคงเหลือ: {Mathf.Ceil(timer)}";

        if (timer <= 0f)
            EndQuest();
    }

    IEnumerator KnockLoop()
    {
        yield return new WaitForSeconds(1f);

        while (isRunning)
        {
            AudioClip clip =
                (Random.value < loudKnockChance && knockLoudClip != null)
                ? knockLoudClip
                : knockSoftClip;

            if (clip != null && DoorAudio != null)
            {
                DoorAudio.pitch = Random.Range(0.95f, 1.05f);
                DoorAudio.PlayOneShot(clip);
            }

            yield return new WaitForSeconds(
                Random.Range(minKnockDelay, maxKnockDelay)
            );
        }
    }

    void EndQuest()
    {
        isRunning = false;

        if (questCanvas != null)
            questCanvas.enabled = false;

        if (knockRoutine != null)
            StopCoroutine(knockRoutine);

        if (DoorAudio != null && kickDoorClip != null)
        {
            DoorAudio.pitch = Random.Range(0.9f, 1.1f);
            DoorAudio.PlayOneShot(kickDoorClip);
        }

        // 🔓 เปิดประตูที่เลือก
        if (doorToOpen != null)
        {
            doorToOpen.OpenDoorByQuest();
        }

        if (failVideo != null)
        {
            failVideo.gameObject.SetActive(true);

            failVideo.loopPointReached += OnVideoFinished;

            failVideo.Play();

            // 🔒 ปิดการควบคุมกล้อง
            FirstPersonController controller = FindObjectOfType<FirstPersonController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            foreach (var go in gameObjectsToDisable)
                if (go != null && go != failVideo.gameObject)
                    go.SetActive(false);
        }
        void OnVideoFinished(VideoPlayer vp)
        {
            // ป้องกันเรียกซ้ำ
            vp.loopPointReached -= OnVideoFinished;
            FirstPersonController controller = FindObjectOfType<FirstPersonController>();
            if (controller != null)
            {
                controller.enabled = true;
            }
            SceneManager.LoadScene("MainMenu");  

        }

        OnQuestTimeEnd?.Invoke();

    }



    public void OnPlayerHide()
    {
        if (questTextGroup != null)
            questTextGroup.SetActive(false);

        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }


    public void OnPlayerExitHide()
    {
        
    }
}
