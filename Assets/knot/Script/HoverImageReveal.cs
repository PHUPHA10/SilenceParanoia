using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverImageReveal : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [Header("Image")]
    public Image hoverImage;
    void Start()
    {
        // ซ่อนรูปภาพไว้ก่อนตอนเริ่มเกม
        if (hoverImage != null)
        {
            hoverImage.enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
        {
            hoverImage.enabled = true; // เปิดการแสดงผลรูปภาพ
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage != null)
        {
            hoverImage.enabled = false; // ปิดการแสดงผลรูปภาพ
        }
    }
}
