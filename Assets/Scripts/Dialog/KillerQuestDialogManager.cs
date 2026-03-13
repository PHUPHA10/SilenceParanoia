using System.Collections;
using UnityEngine;
using TMPro;

public class KillerQuestDialogManager : MonoBehaviour
{
    [Header("References")]
    public QuestTimer questTimer;       // ลาก QuestTimer
    public TMP_Text dialogText;

    [Header("Timing")]
    public float delayBeforeFirstLine = 3f;
    public float displayTime = 4f;
    public float timeBetweenLines = 5f;

    [Header("Ordered Dialogs")]
    [TextArea]
    public string[] dialogs =
    {
        "อาร์ม: ฟ้า อย่าทำแบบนี้ได้ไหม",
        "อาร์ม: เรามาเริ่มต้นใหม่กันไม่ได้หรอ เค้าจะไม่ทำอีกแล้ว",
        "อาร์ม: จำไม่ได้หรอ ทุกอย่างที่เราผ่านมาด้วยกัน เรารักกันแค่ไหน",
        "อาร์ม: อย่าแกล้งทำเป็นไม่ได้ยินเลยฟ้า คิดว่าไม่รู้หรอว่าเธออยู่ในห้องอ่ะ"
    };

    int currentIndex = 0;
    Coroutine dialogRoutine;

    void Start()
    {
        if (dialogText != null)
            dialogText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 🔥 พูดเฉพาะตอน Quest หาที่ซ่อนกำลังรัน
        if (questTimer != null && questTimer.IsQuestRunning)
        {
            if (dialogRoutine == null)
                dialogRoutine = StartCoroutine(DialogSequence());
        }
        else
        {
            StopAll();
        }
    }

    IEnumerator DialogSequence()
    {
        yield return new WaitForSeconds(delayBeforeFirstLine);

        while (currentIndex < dialogs.Length && questTimer.IsQuestRunning)
        {
            dialogText.text = dialogs[currentIndex];
            dialogText.gameObject.SetActive(true);

            yield return new WaitForSeconds(displayTime);

            dialogText.gameObject.SetActive(false);
            currentIndex++;

            yield return new WaitForSeconds(timeBetweenLines);
        }

        dialogRoutine = null;
    }

    void StopAll()
    {
        if (dialogRoutine != null)
        {
            StopCoroutine(dialogRoutine);
            dialogRoutine = null;
        }

        if (dialogText != null)
            dialogText.gameObject.SetActive(false);
    }
}
