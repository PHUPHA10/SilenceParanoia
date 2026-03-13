using UnityEngine;

public class LightSwitchMove : MonoBehaviour
{
    [Header("Switch Movement")]
    public Vector3 moveOffset = new Vector3(0f, -0.03f, 0f);
    public float moveSpeed = 10f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isMoving = false;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + moveOffset;
    }

    public void MoveDown()
    {
        if (!isMoving)
            StartCoroutine(MoveSwitch());
    }

    private System.Collections.IEnumerator MoveSwitch()
    {
        isMoving = true;

        while (Vector3.Distance(transform.localPosition, targetPos) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPos,
                Time.deltaTime * moveSpeed
            );

            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
