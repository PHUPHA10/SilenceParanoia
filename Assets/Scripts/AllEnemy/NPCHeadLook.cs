using UnityEngine;

public class NPCHeadLook : MonoBehaviour
{
    [Header("References")]
    public Transform headBone;
    public Transform player;

    [Header("Settings")]
    public float rotationSpeed = 5f;
    public float maxAngle = 60f;

    Quaternion originalRotation;

    void Start()
    {
        if (headBone != null)
            originalRotation = headBone.localRotation;

        if (player == null)
            player = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (headBone == null || player == null) return;

        Vector3 direction = player.position - headBone.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // จำกัดมุมหมุน
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle < maxAngle)
        {
            headBone.rotation = Quaternion.Slerp(
                headBone.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
        else
        {
            headBone.localRotation = Quaternion.Slerp(
                headBone.localRotation,
                originalRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
