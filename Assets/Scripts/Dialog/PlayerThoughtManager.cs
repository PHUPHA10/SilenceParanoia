using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerThoughtManager : MonoBehaviour
{
    [Header("References")]
    public QuestTimer questTimer;                     // 👈 ลาก QuestTimer
    public KillerHideDialogManager killerDialog;      // 👈 ลาก KillerHideDialogManager
    public TMP_Text thoughtText;

    [Header("Timing")]
    public float displayTime = 3f;
    public float minDelay = 4f;
    public float maxDelay = 8f;

    [Header("Player Thoughts")]
    [TextArea]
    public List<string> thoughts = new List<string>()
    {
        "ฟ้า: ขออย่าให้มันเจอเราเลย...",
        "ฟ้า: หัวใจนี่ก็เต้นแรงจังวะ...",
        "ฟ้า: เมื่อไหร่จะไปซักทีวะเนี่ย..."
    };

    private List<string> unusedThoughts;
    private Coroutine thoughtRoutine;
    private bool questEnded = false;

    float checkTimer = 0f;
    const float CHECK_INTERVAL = 0.2f;

    void Start()
    {
        thoughtText.gameObject.SetActive(false);

        unusedThoughts = new List<string>(thoughts);

        // 🔥 ฟังสัญญาณว่า Quest หมดเวลา
        if (questTimer != null)
            questTimer.OnQuestTimeEnd.AddListener(OnQuestEnded);
    }

    void OnQuestEnded()
    {
        questEnded = true;
    }

    void Update()
    {
        if (!questEnded) return; // ❌ Quest ยังไม่จบ = ห้ามคิด

        checkTimer -= Time.deltaTime;
        if (checkTimer > 0f) return;
        checkTimer = CHECK_INTERVAL;

        if (IsPlayerHiding() && thoughtRoutine == null)
        {
            thoughtRoutine = StartCoroutine(ThoughtLoop());
        }

        if (!IsPlayerHiding() && thoughtRoutine != null)
        {
            StopCoroutine(thoughtRoutine);
            thoughtRoutine = null;
            thoughtText.gameObject.SetActive(false);
        }
    }

    IEnumerator ThoughtLoop()
    {
        while (unusedThoughts.Count > 0 && IsPlayerHiding())
        {
            float wait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(wait);

            // ❗ ถ้าฆาตกรกำลังพูด → เงียบ
            if (killerDialog != null && killerDialog.IsKillerSpeaking)
                continue;

            string thought = GetRandomThought();
            yield return StartCoroutine(ShowThought(thought));
        }

        thoughtRoutine = null;
    }

    IEnumerator ShowThought(string text)
    {
        thoughtText.text = text;
        thoughtText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        thoughtText.gameObject.SetActive(false);
    }

    string GetRandomThought()
    {
        int index = Random.Range(0, unusedThoughts.Count);
        string line = unusedThoughts[index];
        unusedThoughts.RemoveAt(index); // ❗ โผล่แล้วหายถาวร
        return line;
    }

    bool IsPlayerHiding()
    {
        hidingPlace[] hides = FindObjectsOfType<hidingPlace>();

        foreach (var h in hides)
        {
            if (h.IsHiding)
                return true;
        }

        return false;
    }
}
