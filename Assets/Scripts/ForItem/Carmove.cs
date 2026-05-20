using UnityEngine;

public class Carmove : MonoBehaviour
{
    [Header("Move")]
    public float speed = 8f;

    [Tooltip("1 = +X / -1 = -X")]
    public int direction = 1;

    [Header("Destroy")]
    public float destroyAfterSeconds = 20f;

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;

        Destroy(gameObject, destroyAfterSeconds);
    }

    void Update()
    {
        Vector3 pos = transform.position;

        pos.x += speed * direction * Time.deltaTime;

        pos.y = fixedY;
        pos.z = fixedZ;

        transform.position = pos;
    }
}