using UnityEngine;
using System.Collections;

public class DrawerInteract : MonoBehaviour, IInteractable
{
    [Header("Drawer Settings")]
    public Transform drawer;
    public Vector3 openOffset = new Vector3(0f, 0f, 0.6f);
    public float slideSpeed = 2f;

    [Header("Audio")]
    public AudioSource drawerAudio;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private Vector3 closedPos;
    private Coroutine slideRoutine;

    public string Prompt => isOpen ? "ปิดลิ้นชัก" : "เปิดลิ้นชัก";

    void Start()
    {
        if (drawer == null)
            drawer = transform;

        closedPos = drawer.localPosition;

        if (openSound != null) openSound.LoadAudioData();
        if (closeSound != null) closeSound.LoadAudioData();
    }

    public void Interact()
    {
        isOpen = !isOpen;
        PlaySound();

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        Vector3 targetPos = isOpen ? closedPos + openOffset : closedPos;
        slideRoutine = StartCoroutine(SlideDrawer(targetPos));
    }

    IEnumerator SlideDrawer(Vector3 targetPos)
    {
        while (Vector3.Distance(drawer.localPosition, targetPos) > 0.01f)
        {
            drawer.localPosition = Vector3.MoveTowards(
                drawer.localPosition,
                targetPos,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        drawer.localPosition = targetPos;
    }

    void PlaySound()
    {
        if (drawerAudio == null) return;

        drawerAudio.pitch = Random.Range(0.95f, 1.05f);
        drawerAudio.PlayOneShot(isOpen ? openSound : closeSound);
    }
}
