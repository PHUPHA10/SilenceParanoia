using UnityEngine;
using TMPro;

public class SoundMeter : MonoBehaviour
{
    public TMP_Text dbText;

    // ปรับค่านี้ตามไมค์ผู้เล่น (ยิ่งเล็ก = ไว)
    public float sensitivity = 0.06f;

    void Update()
    {
        float loudness = MicrophoneInput.Loudness;

        // กัน log(0)
        loudness = Mathf.Max(loudness, 0.0001f);

        // แปลงเป็น dB
        float db = 20f * Mathf.Log10(loudness / sensitivity);

        // clamp ให้ดูสวย
        db = Mathf.Clamp(db, -60f, 0f);

        if (dbText != null)
            dbText.text = $"ระดับเสียงของคุณ: {Mathf.RoundToInt(db)} dB";
    }
}
