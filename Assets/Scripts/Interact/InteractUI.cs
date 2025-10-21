using UnityEngine;
using TMPro; // สำคัญ! สำหรับ TextMeshPro

public class InteractUI : MonoBehaviour
{
    [SerializeField] TMP_Text promptText; // อ้างถึง TextMeshProUGUI

    // เรียกตอนเล็งเจอของ
    public void Show(string prompt)
    {
        // แสดงเป็น "กด E เพื่อ {Prompt}"
        promptText.text = $"Press E to {prompt}";
        promptText.enabled = true;
    }

    // เรียกตอนเลิกเล็ง
    public void Hide()
    {
        promptText.text = "";
        promptText.enabled = false;
    }
}
