using UnityEngine;
using System.Collections.Generic;

public class RoadBlockManager : MonoBehaviour
{
    public List<Transform> blocks; // ใส่ RoadBlock ทั้งหมด
    public float speed = 20f;
    public float blockLength = 125f;
    public float recycleZ = -150f;

    void Update()
    {
        // เลื่อนทุก block
        foreach (var block in blocks)
        {
            block.Translate(Vector3.back * speed * Time.deltaTime);
        }

        // เช็คตัวหน้าสุด
        Transform firstBlock = blocks[0];

        if (firstBlock.position.z <= recycleZ)
        {
            // เอาตัวหน้าสุดไปไว้หลังสุด
            Transform lastBlock = blocks[blocks.Count - 1];

            Vector3 newPos = lastBlock.position;
            newPos.z += blockLength;

            firstBlock.position = newPos;

            // สลับลำดับใน list
            blocks.RemoveAt(0);
            blocks.Add(firstBlock);
        }
    }
}