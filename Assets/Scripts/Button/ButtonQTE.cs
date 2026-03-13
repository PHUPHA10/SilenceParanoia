using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ButtonQTE : MonoBehaviour
{
    [System.Serializable]
    public class QTESlot
    {
        public KeyCode key;
        public Image keyImage;
        public Image borderImage;
    }

    [Header("QTE Slots")]
    public List<QTESlot> slots = new List<QTESlot>();

    [Header("QTE Settings")]
    public float maxTime = 5f;
    public int maxFails = 3;

    [Header("UI")]
    public TMP_Text failText;   // 🔥 ข้อความจำนวนครั้งที่พลาด

    QTESlot currentSlot;
    float timer;
    int failCount;
    bool active;

    void OnEnable()
    {
        transform.localScale = Vector3.one; // กัน UI หาย
        failCount = 0;
        UpdateFailText();
        StartRound();
    }

    void Update()
    {
        if (!active || currentSlot == null) return;

        timer -= Time.deltaTime;

        if (currentSlot.borderImage)
            currentSlot.borderImage.fillAmount = timer / maxTime;

        if (timer <= 0f)
        {
            RegisterFail();
            return;
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(currentSlot.key))
            {
                StartRound(); // รอด → รอบใหม่
            }
            else
            {
                RegisterFail();
            }
        }
    }

    void StartRound()
    {
        if (slots.Count == 0) return;

        foreach (var s in slots)
        {
            if (s.keyImage) s.keyImage.gameObject.SetActive(false);
            if (s.borderImage) s.borderImage.gameObject.SetActive(false);
        }

        currentSlot = slots[Random.Range(0, slots.Count)];

        if (currentSlot.keyImage)
            currentSlot.keyImage.gameObject.SetActive(true);

        if (currentSlot.borderImage)
        {
            currentSlot.borderImage.gameObject.SetActive(true);
            currentSlot.borderImage.fillAmount = 1f;
        }

        timer = maxTime;
        active = true;
    }

    void RegisterFail()
    {
        failCount++;
        UpdateFailText();

        if (failCount >= maxFails)
        {
            Fail();
        }
        else
        {
            StartRound();
        }
    }

    void UpdateFailText()
    {
        if (failText != null)
            failText.text = $"จำนวนครั้งที่พลาด {failCount} / {maxFails}";
    }

    void Fail()
    {
        active = false;

        if (HidingQTEManager.Instance != null &&
            HidingQTEManager.Instance.CurrentHideSpot != null)
        {
            HidingQTEManager.Instance.CurrentHideSpot.ForceExitHide();
        }

        gameObject.SetActive(false);
    }
}
