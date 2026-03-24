using UnityEngine;
using System.Collections;

public class SlidingDoorSingle : MonoBehaviour
{
    [Header("Door")]
    public Transform door;
    public Vector3 openOffset = new Vector3(-2, 0, 0);
    public float speed = 2f;

    [Header("Timing")]
    public float closeDelay = 2f; // หน่วงก่อนปิด

    [Header("Sound")]
    public AudioSource doorAudio;
    public AudioClip openSound;
    public AudioClip closeSound;

    private Vector3 closedPos;
    private Vector3 openPos;

    private bool isOpen = false;
    private Coroutine closeCoroutine;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (isOpen)
        {
            door.position = Vector3.Lerp(door.position, openPos, Time.deltaTime * speed);
        }
        else
        {
            door.position = Vector3.Lerp(door.position, closedPos, Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ยกเลิกการปิด (ถ้ามี)
            if (closeCoroutine != null)
                StopCoroutine(closeCoroutine);

            OpenDoor();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // เริ่มนับเวลาปิด
            closeCoroutine = StartCoroutine(CloseAfterDelay());
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;

            if (doorAudio != null && openSound != null)
                doorAudio.PlayOneShot(openSound);
        }
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        isOpen = false;

        if (doorAudio != null && closeSound != null)
            doorAudio.PlayOneShot(closeSound);
    }
}