using UnityEngine;
using System.Collections;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [Header("Door Info")]
    [SerializeField] private Animator Door;

    [Header("Animation Durations (seconds)")]
    [SerializeField] private float openDuration = 1.0f;   // ความยาวอนิเมชั่นเปิด
    [SerializeField] private float closeDuration = 1.0f;  // ความยาวอนิเมชั่นปิด

    private bool isOpen = false;
    private bool isAnimating = false;  // กันกดซ้ำระหว่างเล่นอนิเมชั่น

    public string Prompt => isOpen ? "Close Door" : "Open Door";

    public void Interact()
    {
        // ถ้าอนิเมชั่นกำลังเล่นอยู่ ห้ามกดซ้ำ
        if (isAnimating) return;

        if (isOpen)
        {
            // เล่นอนิเมชั่นปิด
            Door.SetTrigger("Close");
            StartCoroutine(DoorRoutine(false));
            isOpen = false;
        }
        else
        {
            // เล่นอนิเมชั่นเปิด
            Door.SetTrigger("Open");
            StartCoroutine(DoorRoutine(true));
            isOpen = true;
        }
    }

    private IEnumerator DoorRoutine(bool opening)
    {
        isAnimating = true;

        // รอเวลาเท่าความยาวคลิป (ตั้งค่าใน Inspector)
        float waitTime = opening ? openDuration : closeDuration;
        yield return new WaitForSeconds(waitTime);

        isAnimating = false;
    }
}
