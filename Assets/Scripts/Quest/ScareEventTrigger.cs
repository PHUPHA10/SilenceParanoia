using System.Collections;
using UnityEngine;

public class CameraEventTrigger : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject playerCamera;
    public GameObject cinematicCamera;

    [Header("Player Control")]
    public MonoBehaviour playerLookScript;

    [Header("Spawn")]
    public GameObject npcPrefab;
    public Transform spawnPoint;

    [Header("NPC Move")]
    public Transform moveTarget;
    public float npcSpeed = 3f;
    public float stopDistance = 0.1f;

    [Header("Timing")]
    public float cinematicDuration = 3f;

    [Header("Settings")]
    public bool triggerOnce = true;

    bool triggered = false;

    void Start()
    {
        // ปิดกล้อง cinematic ตอนเริ่ม
        if (cinematicCamera != null)
            cinematicCamera.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggered && triggerOnce)
            return;

        triggered = true;

        StartCoroutine(EventSequence());
    }

    IEnumerator EventSequence()
    {
        // ปิดการควบคุมกล้องผู้เล่น
        if (playerLookScript != null)
            playerLookScript.enabled = false;

        // สลับกล้อง
        if (playerCamera != null)
            playerCamera.SetActive(false);

        if (cinematicCamera != null)
            cinematicCamera.SetActive(true);

        // Spawn NPC
        GameObject npc = null;

        if (npcPrefab != null && spawnPoint != null)
        {
            npc = Instantiate(
                npcPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
        }

        // ให้ NPC เดิน
        if (npc != null && moveTarget != null)
        {
            StartCoroutine(MoveNPC(npc.transform, moveTarget));
        }

        yield return new WaitForSeconds(cinematicDuration);

        if (cinematicCamera != null)
            cinematicCamera.SetActive(false);

        if (playerCamera != null)
            playerCamera.SetActive(true);

        if (playerLookScript != null)
            playerLookScript.enabled = true;

        if (triggerOnce)
            Destroy(gameObject);
    }

    IEnumerator MoveNPC(Transform npc, Transform target)
    {
        while (npc != null && target != null)
        {
            Vector3 targetPos = new Vector3(
                target.position.x,
                npc.position.y,
                target.position.z
            );

            Vector3 direction = targetPos - npc.position;
            float distance = direction.magnitude;

            // ถึงจุดแล้ว
            if (distance <= stopDistance)
            {
                Destroy(npc.gameObject);
                yield break;
            }

            direction.Normalize();

            npc.position += direction * npcSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                npc.rotation = Quaternion.Slerp(
                    npc.rotation,
                    lookRotation,
                    10f * Time.deltaTime
                );
            }

            yield return null;
        }
    }
}