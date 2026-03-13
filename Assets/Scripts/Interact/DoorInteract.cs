using UnityEngine;
using System.Collections;

public class DoorInteract : MonoBehaviour, IInteractable
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

    private bool isOpen = false;
    private bool isAnimating = false;

    public string Prompt => isOpen ? "ปิดประตู" : "เปิดประตู";

    public void Interact()
    {
        if (isAnimating) return;

        if (isOpen)
        {
            CloseDoorInternal();
        }
        else
        {
            OpenDoorInternal();
        }
    }


    public void OpenDoor()
    {
        if (isAnimating || isOpen) return;
        OpenDoorInternal();
    }


    public void CloseDoor()
    {
        if (isAnimating || !isOpen) return;
        CloseDoorInternal();
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

        float waitTime = opening ? openDuration : closeDuration;
        yield return new WaitForSeconds(waitTime);

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
