using UnityEngine;
using System.Collections.Generic;

public class LightSwitchQuest : MonoBehaviour, IInteractable
{
    [Header("Lights In Room")]
    public List<Light> roomLights = new List<Light>();

    [Header("Light Settings")]
    public float dimIntensity = 0f;
    public float dimSpeed = 2f;

    [Header("Switch Move")]
    public LightSwitchMove switchMove;

    [Header("Quest")]
    public QuestTimer questTimer;

    [Header("TV")]
    public Open tv;

    [Header("Outline")]
    public OutlineItem outline;   // 🔥 ใช้ระบบ Outline ของคุณ

    private bool isActivated = false;
    private bool isLocked = true;
    public bool IsPowerOff => isActivated;

    public string Prompt
    {
        get
        {
            if (isActivated) return "";

            if (isLocked)
                return "ปิดเบรกเกอร์ (สำรวจห้องก่อน)";

            return "ปิดการทำงานเบรกเกอร์";
        }
    }

    void Start()
    {
        if (outline != null)
            outline.SetOutline(false);
    }

    // 🔓 เรียกจาก RoomExploreQuest
    public void SetLocked(bool locked)
    {
        isLocked = locked;

        // เมื่อปลดล็อกเควส → เปิดขอบเหลือง
        if (!locked && !isActivated && outline != null)
            outline.SetOutline(true);
    }

    public void Interact()
    {
        if (isLocked || isActivated) return;

        isActivated = true;

        // ❌ ปิดขอบเมื่อกดแล้ว
        if (outline != null)
            outline.SetOutline(false);

        if (switchMove != null)
            switchMove.MoveDown();

        StartCoroutine(DimLights());

        if (questTimer != null)
            questTimer.StartQuest();

        if (tv != null)
            tv.PowerCut();
    }

    private System.Collections.IEnumerator DimLights()
    {
        float[] startIntensities = new float[roomLights.Count];

        for (int i = 0; i < roomLights.Count; i++)
            if (roomLights[i] != null)
                startIntensities[i] = roomLights[i].intensity;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * dimSpeed;

            for (int i = 0; i < roomLights.Count; i++)
            {
                if (roomLights[i] != null)
                {
                    roomLights[i].intensity =
                        Mathf.Lerp(startIntensities[i], dimIntensity, t);
                }
            }

            yield return null;
        }
    }
}
