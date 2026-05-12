using UnityEngine;
using System.Collections.Generic;

public class RoadBlockManager : MonoBehaviour
{
    public List<Transform> blocks;
    public float speed = 20f;
    public float blockLength = 125f;
    public float recycleZ = -150f;

    void Update()
    {
        // เลื่อนแค่แกน Z
        foreach (var block in blocks)
        {
            Vector3 pos = block.position;

            pos.z -= speed * Time.deltaTime;

            block.position = pos;
        }

        // เช็คตัวหน้าสุด
        Transform firstBlock = blocks[0];

        if (firstBlock.position.z <= recycleZ)
        {
            // เอาตัวหน้าสุดไปไว้หลังสุด
            Transform lastBlock = blocks[blocks.Count - 1];

            Vector3 newPos = firstBlock.position;

            // ล็อค X,Y เดิม
            newPos.x = firstBlock.position.x;
            newPos.y = firstBlock.position.y;

            // เปลี่ยนแค่ Z
            newPos.z = lastBlock.position.z + blockLength;

            firstBlock.position = newPos;

            // สลับลำดับใน List
            blocks.RemoveAt(0);
            blocks.Add(firstBlock);
        }
    }
}