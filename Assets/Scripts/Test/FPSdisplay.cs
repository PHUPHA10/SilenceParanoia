using UnityEngine;
using TMPro;

public class FPSdisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField, Tooltip("ค่ายิ่งสูงยิ่งไหลช้า (หน่วงมาก)")]
    private float smoothing = 0.9f; // 0..0.99 (แนะนำ 0.9–0.98)

    private float smoothedDelta = 1f/60f; // เริ่มต้นประมาณ 60 FPS

    void Update()
    {
        float d = Time.unscaledDeltaTime;
        // EMA ของ delta time
        smoothedDelta = Mathf.Lerp(d, smoothedDelta, smoothing);
        float fps = 1f / Mathf.Max(0.0001f, smoothedDelta);

        if (label) label.text = $"{fps:0} FPS";
    }
}
