using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;

    private Material materialInstance;

    // สีแดงเลือด (เข้มกว่า Color.red)
    private Color bloodRed = new Color(0.6f, 0f, 0f);
    private Color glowRed = new Color(1f, 0.1f, 0.1f);

    void Start()
    {
        materialInstance = Instantiate(text.fontMaterial);
        text.fontMaterial = materialInstance;

        // ตั้งค่าเริ่มต้น
        text.color = Color.white;
        materialInstance.SetFloat("_GlowPower", 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = bloodRed;

        materialInstance.SetColor("_GlowColor", glowRed);
        materialInstance.SetFloat("_GlowPower", 0.6f);
        materialInstance.SetFloat("_GlowOuter", 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.white;
        materialInstance.SetFloat("_GlowPower", 0f);
    }
}
