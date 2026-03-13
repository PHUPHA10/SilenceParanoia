using UnityEngine;
using StarterAssets;

public class PlayerFootstepSound : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController controller;
    public CharacterController characterController;
    public PlayerData playerData;
    public AudioSource footstepAudio;

    [Header("Footstep Sounds")]
    public AudioClip[] walkClips;
    public AudioClip[] runClips;

    [Header("Step Settings")]
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.35f;
    public float moveThreshold = 0.05f;

    float stepTimer = 0f;
    bool wasMovingLastFrame = false;
    float lastMoveTime = 0f;   // ⭐ กันเสียงหลุดเฟรมสุดท้าย

    void Start()
    {
        if (controller == null)
            controller = GetComponent<FirstPersonController>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (playerData == null)
            playerData = GetComponent<PlayerData>();
    }

    void Update()
    {
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        // 🔇 ซ่อน = เงียบสนิท
        if (playerData != null && playerData.isHiding)
        {
            ResetFootstep();
            return;
        }

        Vector3 velocity = characterController.velocity;
        velocity.y = 0f;

        float speed = velocity.magnitude;
        bool isMoving = speed > moveThreshold;

        // 🛑 หยุดเดิน / หยุดวิ่ง → ตัดเสียงทันที
        if (!isMoving)
        {
            ResetFootstep();
            return;
        }

        // จำเวลาที่ยังเคลื่อนที่อยู่
        lastMoveTime = Time.time;

        // ▶ เพิ่งเริ่มเคลื่อนที่
        if (!wasMovingLastFrame)
        {
            stepTimer = 0f;
            wasMovingLastFrame = true;
        }

        bool isRunning = speed > 2.5f; // ค่า sprint default ของ StarterAssets
        float interval = isRunning ? runStepInterval : walkStepInterval;

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            // ⭐ buffer กันเสียงหลุด (เฟรมสุดท้าย)
            if (Time.time - lastMoveTime > 0.05f)
                return;

            PlayFootstep(isRunning);
            stepTimer = interval;
        }
    }

    void ResetFootstep()
    {
        stepTimer = 0f;
        wasMovingLastFrame = false;

        // ⭐ หยุดเสียงที่กำลังเล่นอยู่ทันที
        if (footstepAudio != null && footstepAudio.isPlaying)
            footstepAudio.Stop();
    }

    void PlayFootstep(bool running)
    {
        if (footstepAudio == null) return;

        AudioClip clip = null;

        if (running && runClips.Length > 0)
            clip = runClips[Random.Range(0, runClips.Length)];
        else if (!running && walkClips.Length > 0)
            clip = walkClips[Random.Range(0, walkClips.Length)];

        if (clip == null) return;

        footstepAudio.pitch = Random.Range(0.96f, 1.04f);
        footstepAudio.PlayOneShot(clip);
    }
}
