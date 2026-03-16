using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundQTE : MonoBehaviour
{
    [Header("Sound Limit (dB)")]
    public float failDB = -5f;
    public float minDB = -40f;
    public float sensitivity = 0.2f;

    [Header("UI")]
    public TMP_Text dbText;
    public TMP_Text hintText;
    public Image emptyImage;
    public Image fullImage;

    [Header("Visual")]
    public float riseSpeed = 20f;      // ขึ้นเร็ว
    public float fallSmoothSpeed = 6f; // ลงช้ากว่า

    private bool active;
    private float currentFill;

    void OnEnable()
    {
        active = true;
        MicrophoneInput.StartMic();

        if (hintText != null)
            hintText.text = "ห้ามส่งเสียงเด็ดขาด...";

        currentFill = 0f;

        if (fullImage != null)
            fullImage.fillAmount = 0f;
    }

    void OnDisable()
    {
        MicrophoneInput.StopMic();
    }

    void Update()
    {
        if (!active) return;

        float loudness = Mathf.Max(MicrophoneInput.Loudness, 0.0001f);

        float db = 20f * Mathf.Log10(loudness / sensitivity);
        db = Mathf.Clamp(db, -60f, 0f);

        if (dbText != null)
            dbText.text = $"ระดับเสียง: {Mathf.RoundToInt(db)} dB";

        float targetFill = Mathf.InverseLerp(minDB, failDB, db);

        // ถ้าเสียงเกินลิมิต ให้แดงเต็มทันที
        if (db >= failDB)
        {
            currentFill = 1f;

            if (fullImage != null)
                fullImage.fillAmount = 1f;

            Fail();
            return;
        }

        // เสียงดังขึ้น -> ขึ้นเร็วมาก
        if (targetFill > currentFill)
        {
            currentFill = Mathf.MoveTowards(currentFill, targetFill, riseSpeed * Time.deltaTime);
        }
        // เสียงเบาลง -> ค่อย ๆ ลด
        else
        {
            currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * fallSmoothSpeed);
        }

        if (fullImage != null)
            fullImage.fillAmount = currentFill;
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