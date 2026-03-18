using UnityEngine;

public class RoadBlockMover : MonoBehaviour
{
    public float speed = 20f;
    public float blockLength = 125f;
    public float recycleZ = -150f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z <= recycleZ)
        {
            Vector3 pos = transform.position;

            pos.z += blockLength; // ✅ ขยับทีละบล็อก

            transform.position = pos;
        }
    }
}