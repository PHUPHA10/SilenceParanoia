using UnityEngine;
using TMPro;
using System.Collections;

public class QuestReminderDialogManager : MonoBehaviour
{
    [Header("References")]
    public RoomExploreQuest exploreQuest;
    public QuestTimer questTimer;
    public TMP_Text dialogText;

    [Header("Timing")]
    public float exploreReminderDelay = 25f;
    public float repeatInterval = 8f;

    Coroutine reminderRoutine;

    void Start()
    {
        if (dialogText != null)
            dialogText.gameObject.SetActive(false);

        reminderRoutine = StartCoroutine(ReminderLoop());
    }

    void Update()
    {
        // 🔥 ถ้าสำรวจเสร็จแล้ว → ปิดข้อความทันที
        if (!IsExploreQuestActive() && dialogText.gameObject.activeSelf)
        {
            dialogText.gameObject.SetActive(false);
        }
    }

    IEnumerator ReminderLoop()
    {
        // ---------- เควสสำรวจห้อง ----------
        yield return new WaitForSeconds(exploreReminderDelay);

        while (IsExploreQuestActive())
        {
            ShowDialog("รีบสำรวจความผิดปกติในห้องดีกว่า เผื่อมันแอบมาซ่อนตัวอยู่");

            float timer = 0f;
            while (timer < repeatInterval)
            {
                // ❗ ถ้าเควสเสร็จระหว่างรอ → ออกทันที
                if (!IsExploreQuestActive())
                    yield break;

                timer += Time.deltaTime;
                yield return null;
            }
        }

        // ---------- เควสปิดเบรกเกอร์ ----------
        while (IsBreakerQuestActive())
        {
            ShowDialog("ต้องรีบไปปิดเบรกเกอร์แล้ว ไม่อย่างนั้นมันรู้แน่ว่าเราอยู่ห้อง");
            yield return new WaitForSeconds(repeatInterval);
        }

        // ---------- เควสหาที่ซ่อน ----------
        while (IsHideQuestCritical())
        {
            ShowDialog("ต้องรีบหาที่ซ่อนแล้ว ไม่อย่างนั้นมันบุกเข้ามาในห้องแน่");
            yield return new WaitForSeconds(repeatInterval);
        }

        dialogText.gameObject.SetActive(false);
    }

    // =======================
    // Condition Checks
    // =======================

    bool IsExploreQuestActive()
    {
        return exploreQuest != null && exploreQuest.exploreGroup.activeSelf;
    }

    bool IsBreakerQuestActive()
    {
        return exploreQuest != null &&
               !exploreQuest.exploreGroup.activeSelf &&
               exploreQuest.breakerQuestGroup.activeSelf;
    }

    bool IsHideQuestCritical()
    {
        return questTimer != null &&
               questTimer.IsQuestRunning &&
               GetRemainingTime() <= 15f;
    }

    float GetRemainingTime()
    {
        if (questTimer.timerText == null) return 999f;

        float.TryParse(
            questTimer.timerText.text.Replace("เวลาคงเหลือ:", "").Trim(),
            out float time
        );

        return time;
    }

    // =======================
    // UI
    // =======================

    void ShowDialog(string text)
    {
        dialogText.text = text;
        dialogText.gameObject.SetActive(true);
    }
}
