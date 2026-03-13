using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomExploreQuest quest;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        // ถ้าเคยใช้แล้ว → ไม่ทำงานอีก
        if (used) return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        if (quest != null)
            quest.OnEnterRoom(GetComponent<Collider>());

        // 🔥 ทำลาย trigger นี้ทิ้ง เพื่อไม่ให้ซ้ำ
        Destroy(gameObject);
    }
}
