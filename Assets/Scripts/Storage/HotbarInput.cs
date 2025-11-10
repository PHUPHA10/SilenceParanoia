using UnityEngine;

public class HotbarInput : MonoBehaviour
{
    void Update()
    {
        // เลข 1..5 (หรือเท่ากับ hotbarSize)
        for (int i = 0; i < HotbarModel.Instance.hotbarSize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                HotbarModel.Instance.SelectIndex(i);
                Debug.Log("Work"+i);
            }
        }

        // สกรอลล์เมาส์
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) HotbarModel.Instance.SelectNextFilled(-1); // ขึ้นบน = ย้อน
        else if (scroll < 0f) HotbarModel.Instance.SelectNextFilled(+1);
        
    }
}
