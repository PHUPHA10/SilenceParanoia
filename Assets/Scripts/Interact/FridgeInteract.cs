using UnityEngine;
using System.Collections;

public class FridgeInteract : MonoBehaviour, IInteractable
{
    [Header("Door")]
    public Transform door;
    public float openAngle = 90f;
    public float rotateSpeed = 2f;

    [Header("Audio")]
    public AudioSource fridgeAudio;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private Coroutine rotateRoutine;

    public string Prompt => isOpen ? "ปิดตู้เย็นช่องธรรมดา" : "เปิดตู้เย็นช่องธรรมดา";

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
        Quaternion startRot = door.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, targetAngle, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotateSpeed;
            door.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        door.localRotation = targetRot;
    }

    void PlaySound()
    {
        if (fridgeAudio == null) return;

        fridgeAudio.pitch = Random.Range(0.95f, 1.05f);
        fridgeAudio.PlayOneShot(isOpen ? openSound : closeSound);
    }
}
