using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundQTE : MonoBehaviour
{
    [Header("Sound Limit (dB)")]
    public float failDB = -5f;          // ถ้าเสียงดังกว่านี้ → แพ้
    public float sensitivity = 0.05f;    // ปรับตามไมค์

    [Header("UI")]
    public TMP_Text dbText;
    public TMP_Text hintText;
    public Image dangerFill;            // optional (เช่น bar วัดเสียง)

    bool active;

    void OnEnable()
    {
        active = true;
        MicrophoneInput.StartMic();

        if (hintText != null)
            hintText.text = "ถ้าอยากมีชีวิต ห้ามส่งเสียงเด็ดขาด...";

        if (dangerFill != null)
            dangerFill.fillAmount = 0f;
    }

    void OnDisable()
    {
        MicrophoneInput.StopMic();
    }

    void Update()
    {
        if (!active) return;

        float loudness = MicrophoneInput.Loudness;
        loudness = Mathf.Max(loudness, 0.0001f);

        float db = 20f * Mathf.Log10(loudness / sensitivity);
        db = Mathf.Clamp(db, -60f, 0f);

        // UI
        if (dbText != null)
            dbText.text = $"ระดับเสียง: {Mathf.RoundToInt(db)} dB";

        if (dangerFill != null)
            dangerFill.fillAmount = Mathf.InverseLerp(-40f, failDB, db);

        // ❌ ดังเกิน → แพ้
        if (db > failDB)
        {
            Fail();
        }
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
