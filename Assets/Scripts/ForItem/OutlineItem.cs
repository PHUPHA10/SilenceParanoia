using UnityEngine;

public class OutlineItem : MonoBehaviour
{
    private Material[] materials;

    void Start()
    {
        // ดึงทุก material ของตัวนี้
        materials = GetComponent<Renderer>().materials;

        // ปิดขอบตอนเริ่มเกม
        SetOutline(false);
    }

    public void SetOutline(bool on)
    {

        if (materials == null) return;

        foreach (var mat in materials)
        {
            if (mat != null && mat.HasProperty("_outline_scale"))
            {
                mat.SetFloat("_outline_scale", on ? 1.04f : 0f);
            }
        }
    }
}
