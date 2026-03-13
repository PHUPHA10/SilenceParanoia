using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillerHideDialogManager : MonoBehaviour
{
    [Header("References")]
    public QuestTimer questTimer; 
    public TMP_Text dialogText;

    [Header("Timing")]
    public float delayAfterQuestEnd = 5f;
    public float displayTime = 4f;
    public float timeBetweenDialogs = 7f;

    [Header("Killer Dialogs")]
    [TextArea]
    public List<string> killerDialogs = new List<string>()
    {
        "อาร์ม: อยู่ไหนวะฟ้า…",
        "อาร์ม: เลิกซ่อนแล้วออกมาคุยกันหน่อย…",
        "อาร์ม: รู้นะว่าอยู่ในนี้…"
    };

    public bool IsKillerSpeaking { get; private set; }

    private List<string> unusedDialogs;
    private Coroutine dialogRoutine;
    private bool questEnded = false;

    float checkTimer = 0f;
    const float CHECK_INTERVAL = 0.2f;

    void Start()
    {
        unusedDialogs = new List<string>(killerDialogs);

        if (dialogText != null)
            dialogText.gameObject.SetActive(false);

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
        if (!questEnded) return; // ❌ Quest ยังไม่จบ = ห้ามพูด

        checkTimer -= Time.deltaTime;
        if (checkTimer > 0f) return;
        checkTimer = CHECK_INTERVAL;

        if (IsPlayerHiding() && dialogRoutine == null)
        {
            dialogRoutine = StartCoroutine(DialogSequence());
        }

        if (!IsPlayerHiding() && dialogRoutine != null)
        {
            StopCoroutine(dialogRoutine);
            dialogRoutine = null;

            dialogText.gameObject.SetActive(false);
            IsKillerSpeaking = false;
        }
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

    IEnumerator DialogSequence()
    {
        // รอหลัง Quest หมด
        yield return new WaitForSeconds(delayAfterQuestEnd);

        while (unusedDialogs.Count > 0 && IsPlayerHiding())
        {
            string dialog = GetRandomDialog();
            yield return StartCoroutine(KillerSpeak(dialog));

            yield return new WaitForSeconds(timeBetweenDialogs);
        }

        dialogRoutine = null;
    }

    IEnumerator KillerSpeak(string text)
    {
        IsKillerSpeaking = true;

        dialogText.text = text;
        dialogText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        dialogText.gameObject.SetActive(false);
        IsKillerSpeaking = false;
    }

    string GetRandomDialog()
    {
        int index = Random.Range(0, unusedDialogs.Count);
        string line = unusedDialogs[index];
        unusedDialogs.RemoveAt(index); // ❗ ไม่พูดซ้ำ
        return line;
    }
}
