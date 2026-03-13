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
            float maxZ = GetMaxZPosition();
            Vector3 pos = transform.position;
            pos.z = maxZ + blockLength;
            transform.position = pos;
        }
    }

    float GetMaxZPosition()
    {
        RoadBlockMover[] blocks = FindObjectsOfType<RoadBlockMover>();

        float maxZ = float.MinValue;

        foreach (var block in blocks)
        {
            if (block.transform.position.z > maxZ)
                maxZ = block.transform.position.z;
        }

        return maxZ;
    }
}
