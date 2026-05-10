using System.Collections;
using UnityEngine;

public class CameraEventTrigger : MonoBehaviour
{
    [Header("Player Camera")]
    public GameObject playerCamera;

    [Header("Cinematic Cameras")]
    public GameObject cinematicCamera1;
    public GameObject cinematicCamera2;

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

    [Header("Disable Other Trigger")]
    public GameObject otherTrigger;

    [Header("Destroy Object After Cinematic")]
    public GameObject objectToDestroy;

    [Header("Settings")]
    public bool useCamera1 = true;
    public bool triggerOnce = true;

    bool triggered = false;

    void Start()
    {

        if (cinematicCamera1 != null)
            cinematicCamera1.SetActive(false);

        if (cinematicCamera2 != null)
            cinematicCamera2.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggered && triggerOnce)
            return;

        triggered = true;

        if (otherTrigger != null)
        {
            otherTrigger.SetActive(false);
        }

        StartCoroutine(EventSequence());
    }

    IEnumerator EventSequence()
    {
        if (playerLookScript != null)
            playerLookScript.enabled = false;

        if (playerCamera != null)
            playerCamera.SetActive(false);


        if (useCamera1)
        {
            if (cinematicCamera1 != null)
                cinematicCamera1.SetActive(true);
        }
        else
        {
            if (cinematicCamera2 != null)
                cinematicCamera2.SetActive(true);
        }

        GameObject npc = null;

        if (npcPrefab != null && spawnPoint != null)
        {
            npc = Instantiate(
                npcPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            Destroy(npc, 15f);
        }

        if (npc != null && moveTarget != null)
        {
            StartCoroutine(MoveNPC(npc.transform, moveTarget));
        }

        yield return new WaitForSeconds(cinematicDuration);

        if (cinematicCamera1 != null)
            cinematicCamera1.SetActive(false);

        if (cinematicCamera2 != null)
            cinematicCamera2.SetActive(false);

        if (playerCamera != null)
            playerCamera.SetActive(true);

        if (playerLookScript != null)
            playerLookScript.enabled = true;

        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy.gameObject);
        }

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


            Destroy(npc.gameObject, 13f);

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