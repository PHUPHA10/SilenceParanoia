using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CharacterNameButton : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI text;
    public int characterID; // 0 = ฟ้า, 1 = อาร์ม

    private CharacterSelectionManager manager;
    private bool isSelected = false;
    private Material materialInstance;

    // สีเลือด
    private Color bloodRed = new Color(0.6f, 0f, 0f);
    private Color glowRed = new Color(1f, 0.1f, 0.1f);

    void Start()
    {
        manager = FindObjectOfType<CharacterSelectionManager>();

        // สร้าง material instance เพื่อไม่ให้กระทบตัวอื่น
        materialInstance = Instantiate(text.fontMaterial);
        text.fontMaterial = materialInstance;

        SetNormal();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
            SetHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
            SetNormal();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.Select(this);
    }

    // ====== STATE FUNCTIONS ======

    public void SetSelected()
    {
        isSelected = true;

        text.color = bloodRed;

        materialInstance.SetColor("_GlowColor", glowRed);
        materialInstance.SetFloat("_GlowPower", 0.6f);
        materialInstance.SetFloat("_GlowOuter", 0.4f);
    }

    public void SetNormal()
    {
        isSelected = false;

        text.color = Color.white;
        materialInstance.SetFloat("_GlowPower", 0f);
    }

    private void SetHover()
    {
        text.color = bloodRed;

        materialInstance.SetColor("_GlowColor", glowRed);
        materialInstance.SetFloat("_GlowPower", 0.5f);
        materialInstance.SetFloat("_GlowOuter", 0.3f);
    }
}
