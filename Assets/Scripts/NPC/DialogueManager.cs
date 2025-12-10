using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Timing (seconds)")]
    [SerializeField] private float defaultSecondsPerLine = 2.5f;
    public MonoBehaviour[] componentsToDisable;

    string speaker;
    string[] lines;
    float[] perLineDurations;
    int index;
    bool isActive;
    bool skipRequested;
    Coroutine runCo;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;

        // ✔ ใช้ Enter ข้าม (ทั้งคีย์ Enter ปกติและ Numpad Enter)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            skipRequested = true;
        }
    }

    // ใช้เวลาเท่ากันทุกบรรทัด
    public void StartDialogue(string npcName, string[] dialogueLines, float secondsPerLine = -1f)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;
        if (isActive) StopCurrent();

        speaker = npcName;
        lines = dialogueLines;
        perLineDurations = null;
        if (secondsPerLine > 0f) defaultSecondsPerLine = secondsPerLine;

        BeginAndRun();
    }

    // กำหนดเวลาต่อบรรทัด
    public void StartDialogue(string npcName, string[] dialogueLines, float[] durationsPerLine)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;
        if (durationsPerLine == null || durationsPerLine.Length != dialogueLines.Length)
        {
            Debug.LogWarning("durationsPerLine length must match dialogueLines length. Fallback to defaultSecondsPerLine.");
            StartDialogue(npcName, dialogueLines, defaultSecondsPerLine);
            return;
        }
        if (isActive) StopCurrent();

        speaker = npcName;
        lines = dialogueLines;
        perLineDurations = durationsPerLine;

        BeginAndRun();


    }

    void BeginAndRun()
    {
        index = 0;
        isActive = true;
        skipRequested = false;

        if (dialoguePanel) dialoguePanel.SetActive(true);
        if (nameText) nameText.text = speaker;

        if (runCo != null) StopCoroutine(runCo);
        runCo = StartCoroutine(RunRoutine());

        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null)
        {
            // ซ่อน Prompt ตอนเริ่มคุย (ถ้าอยากให้หายทันที)
            playerInteract.HidePrompt();   // ถ้าเมทอดนี้ยังเป็น private ให้เปลี่ยนเป็น public ก่อน

            // ปิดการทำงานของ PlayerInteract ชั่วคราว
            playerInteract.enabled = false;
        }

    }

    IEnumerator RunRoutine()
    {
        while (index < lines.Length)
        {
            string line = lines[index] ?? "";
            if (dialogueText) dialogueText.text = line;

            float wait = perLineDurations != null
                ? Mathf.Max(0.05f, perLineDurations[index])
                : Mathf.Max(0.05f, defaultSecondsPerLine);

            float t = 0f; skipRequested = false;
            while (t < wait && !skipRequested)
            {
                t += Time.deltaTime;
                yield return null;
            }

            index++;
        }

        End();
    }

    void StopCurrent()
    {
        if (runCo != null) { StopCoroutine(runCo); runCo = null; }
        isActive = false;
        skipRequested = false;
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    void End()
    {
        StopCurrent();
        lines = null; perLineDurations = null; speaker = null;

        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null)
        {
            playerInteract.enabled = true;
        }

    }
}
