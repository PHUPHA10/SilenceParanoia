using UnityEngine;

public class CarAutoMove : MonoBehaviour
{
    public float speed = 6f;
    public float rotationSpeed = 4f;

    [Header("Waypoints")]
    public Transform[] waypoints;

    private int currentIndex = 0;

    private void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];

        // 🔹 คำนวณทิศทาง (ตัดแกน Y ออก)
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        // 🔹 หมุนรถ (ไม่ให้เงย/ก้ม)
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // 🔹 เคลื่อนที่เฉพาะพื้น (ไม่ลอย)
        Vector3 moveDir = transform.forward;
        moveDir.y = 0f;

        transform.position += moveDir.normalized * speed * Time.deltaTime;

        // 🔹 เช็คระยะ (ตัดแกน Y)
        Vector3 flatTarget = target.position;
        flatTarget.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, flatTarget);

        if (distance < 1f)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                currentIndex = waypoints.Length - 1;
                // หรือวน:
                // currentIndex = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Erase"))
        {
            Destroy(gameObject);
        }
    }
}