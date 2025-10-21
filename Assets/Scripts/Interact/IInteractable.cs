using UnityEngine;

public interface IInteractable
{
    string Prompt { get ; }       // ข้อความจะแสดงใน UI หรือ Console
    void Interact();             // ฟังก์ชันที่เรียกเมื่อกด E
}

