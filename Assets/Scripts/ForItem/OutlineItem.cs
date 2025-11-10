using UnityEngine;

public class OutlineItem : MonoBehaviour
{
    private Material[] materials;

    void Start()
    {
        // ดึง materials ทั้งหมดจาก renderer (สำเนา instanced แล้ว)
        materials = GetComponent<Renderer>().materials;
    }

    void Update()
    {
        // กด Q เพื่อเปิด/ปิดขอบ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetOutlineScale(0f);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            SetOutlineScale(1.04f);
        }
    }

    public void SetOutlineScale(float value)
    {
        if (materials == null) return;

        // loop ทุก material แล้วตั้งค่าพร้อมกัน
        foreach (var mat in materials)
        {
            if (mat.HasProperty("_outline_scale"))
                mat.SetFloat("_outline_scale", value);
        }
    }
}
