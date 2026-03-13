using UnityEngine;
using System.Collections;

public class SlidingDoorInteract2 : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public Transform door;
    public Vector3 openOffset = new Vector3(1.2f, 0f, 0f); // เลื่อนอีกฝั่ง
    public float slideSpeed = 2f;

    [Header("Door Audio")]
    public AudioSource doorAudio;
    public AudioClip slideOpenSound;
    public AudioClip slideCloseSound;

    [Header("Outside Ambient")]
    public AudioSource outsideAmbient;        // 🔊 เสียงภายนอก
    [Range(0f, 1f)]
    public float outsideTargetVolume = 0.6f;  // ⭐ ปรับความดังเอง
    public float ambientFadeSpeed = 1.5f;

    private bool isOpen = false;
    private bool outsidePlaying = false;
    private Vector3 closedPos;
    private Coroutine slideRoutine;
    private Coroutine ambientRoutine;

    public string Prompt => isOpen ? "ปิดประตู" : "เปิดประตู";

    void Start()
    {
        if (door == null)
            door = transform;

        closedPos = door.localPosition;

        // preload กันดีเลย์
        if (slideOpenSound != null) slideOpenSound.LoadAudioData();
        if (slideCloseSound != null) slideCloseSound.LoadAudioData();

        if (outsideAmbient != null)
        {
            outsideAmbient.volume = 0f;
            outsideAmbient.Stop();
        }
    }

    public void Interact()
    {
        isOpen = !isOpen;

        PlayDoorSound();
        HandleOutsideAmbient();

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        Vector3 targetPos = isOpen ? closedPos + openOffset : closedPos;
        slideRoutine = StartCoroutine(SlideDoor(targetPos));
    }

    IEnumerator SlideDoor(Vector3 targetPos)
    {
        while (Vector3.Distance(door.localPosition, targetPos) > 0.01f)
        {
            door.localPosition = Vector3.MoveTowards(
                door.localPosition,
                targetPos,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        door.localPosition = targetPos;
    }

    void PlayDoorSound()
    {
        if (doorAudio == null) return;

        doorAudio.pitch = Random.Range(0.95f, 1.05f);

        if (isOpen && slideOpenSound != null)
            doorAudio.PlayOneShot(slideOpenSound);
        else if (!isOpen && slideCloseSound != null)
            doorAudio.PlayOneShot(slideCloseSound);
    }

    // ⭐ คุมเสียงภายนอกให้เล่นค้างจนกว่าจะปิดประตู
    void HandleOutsideAmbient()
    {
        if (outsideAmbient == null) return;

        if (isOpen && !outsidePlaying)
        {
            outsidePlaying = true;

            if (ambientRoutine != null)
                StopCoroutine(ambientRoutine);

            ambientRoutine = StartCoroutine(FadeOutside(outsideTargetVolume));
        }

        if (!isOpen && outsidePlaying)
        {
            outsidePlaying = false;

            if (ambientRoutine != null)
                StopCoroutine(ambientRoutine);

            ambientRoutine = StartCoroutine(FadeOutside(0f));
        }
    }

    IEnumerator FadeOutside(float targetVolume)
    {
        if (!outsideAmbient.isPlaying)
            outsideAmbient.Play();

        while (!Mathf.Approximately(outsideAmbient.volume, targetVolume))
        {
            outsideAmbient.volume = Mathf.MoveTowards(
                outsideAmbient.volume,
                targetVolume,
                Time.deltaTime * ambientFadeSpeed
            );
            yield return null;
        }

        outsideAmbient.volume = targetVolume;

        if (Mathf.Approximately(targetVolume, 0f))
            outsideAmbient.Stop();
    }
}
