using UnityEngine;
using TMPro;

public class QTEFailsManager : MonoBehaviour
{
    public static QTEFailsManager Instance { get; private set; }

    [Header("Fail Settings")]
    public int maxFails = 3;

    [Header("UI")]
    public TMP_Text failText;   // ลาก Failtext (TMP) ใส่ตรงนี้

    private int currentFails = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        // 🔥 บังคับเปิด UI ทุกครั้งที่ถูกเปิด
        if (failText != null)
            failText.gameObject.SetActive(true);

        UpdateUI();
    }

    public void ResetFails()
    {
        currentFails = 0;

        if (failText != null)
            failText.gameObject.SetActive(true);

        UpdateUI();
    }

    public void RegisterFail()
    {
        currentFails++;
        UpdateUI();

        if (currentFails >= maxFails)
        {
            ForceFailAll();
        }
    }

    void UpdateUI()
    {
        if (failText == null) return;

        // 🔥 บังคับเปิดเสมอ (กันโดนปิดจาก QTE อื่น)
        if (!failText.gameObject.activeSelf)
            failText.gameObject.SetActive(true);

        failText.text = $"จำนวนครั้งที่พลาด {currentFails} / {maxFails}";
    }

    void ForceFailAll()
    {
        Debug.Log("❌ QTE FAIL (GLOBAL)");

        if (HidingQTEManager.Instance != null &&
            HidingQTEManager.Instance.CurrentHideSpot != null)
        {
            HidingQTEManager.Instance.CurrentHideSpot.ForceExitHide();
        }
    }
}
