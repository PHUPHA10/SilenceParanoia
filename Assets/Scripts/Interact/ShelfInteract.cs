using UnityEngine;
using System.Collections;

public class ShelfInteract : MonoBehaviour, IInteractable
{
    [Header("Door")]
    public Transform shelf;
    public float openAngle = 90f;
    public float rotateSpeed = 2f;

    [Header("Audio")]
    public AudioSource shelfAudio;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private Coroutine rotateRoutine;

    public string Prompt => isOpen ? "ปิดตู้" : "เปิดตู้";

    void Start()
    {
        if (openSound != null) openSound.LoadAudioData();
        if (closeSound != null) closeSound.LoadAudioData();
    }

    public void Interact()
    {
        isOpen = !isOpen;
        PlaySound();

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateDoor(isOpen ? openAngle : 0f));
    }

    IEnumerator RotateDoor(float targetAngle)
    {
        Quaternion startRot = shelf.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, targetAngle, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotateSpeed;
            shelf.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        shelf.localRotation = targetRot;
    }

    void PlaySound()
    {
        if (shelfAudio == null) return;

        shelfAudio.pitch = Random.Range(0.95f, 1.05f);
        shelfAudio.PlayOneShot(isOpen ? openSound : closeSound);
    }
}
