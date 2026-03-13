using UnityEngine;
using System.Collections;

public class Doorquest : MonoBehaviour, IInteractable
{
    [Header("Door Info")]
    [SerializeField] private Animator Door;

    [Header("Animation Durations (seconds)")]
    [SerializeField] private float openDuration = 1.0f;
    [SerializeField] private float closeDuration = 1.0f;

    [Header("Audio")]
    [SerializeField] private AudioSource doorAudio;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    [Header("Lock Settings")]
    [SerializeField] private bool lockInteractAtStart = true; // 🔒 ล็อกตั้งแต่เริ่มเกม

    private bool isOpen = false;
    private bool isAnimating = false;
    private bool isLocked;

    public string Prompt
    {
        get
        {
            if (isLocked) return ""; // ไม่โชว์ข้อความกด E
            return isOpen ? "ปิดประตู" : "เปิดประตู";
        }
    }

    void Start()
    {
        isLocked = lockInteractAtStart;
    }


    public void Interact()
    {
        if (isLocked) return;
        if (isAnimating) return;

        if (isOpen)
            CloseDoorInternal();
        else
            OpenDoorInternal();
    }


    public void OpenDoorByQuest()
    {
        if (isAnimating) return;

        isLocked = false; // ปลดล็อก
        if (!isOpen)
            OpenDoorInternal();
    }


    public void LockDoor()
    {
        isLocked = true;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }



    void OpenDoorInternal()
    {
        Door.SetTrigger("Open");
        PlayOpenSound();
        StartCoroutine(DoorRoutine(true));
        isOpen = true;
    }

    void CloseDoorInternal()
    {
        Door.SetTrigger("Close");
        PlayCloseSound();
        StartCoroutine(DoorRoutine(false));
        isOpen = false;
    }

    private IEnumerator DoorRoutine(bool opening)
    {
        isAnimating = true;
        yield return new WaitForSeconds(opening ? openDuration : closeDuration);
        isAnimating = false;
    }

    void PlayOpenSound()
    {
        if (doorAudio != null && openSound != null)
        {
            doorAudio.pitch = Random.Range(0.95f, 1.05f);
            doorAudio.PlayOneShot(openSound);
        }
    }

    void PlayCloseSound()
    {
        if (doorAudio != null && closeSound != null)
        {
            doorAudio.pitch = Random.Range(0.95f, 1.05f);
            doorAudio.PlayOneShot(closeSound);
        }
    }
}
