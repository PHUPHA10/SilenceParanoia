using UnityEngine;

public class PointerController : MonoBehaviour
{
    [Header("Pointer Move")]
    public Transform pointA;              // ????????
    public Transform pointB;              // ???????
    public RectTransform safeZone;        // ??????????
    public float moveSpeed = 100f;        // ???????? pointer

    [Header("QTE Logic")]
    public int maxFails = 3;              // ???????????????
    public GameObject qteRoot;            // Root UI ??? QTE (Panel ??????? pointer + bar)
    public EnemyAI enemyAI;               // ?????????????????? Inspector

    [Header("Hiding Target")]
    public Transform hidingTarget;        // ??????????????????????? (???? ?????????? / ???????)

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private int failCount = 0;
    private bool qteActive = true;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();

        // ??????????????? pointB ????
        if (pointB != null)
            targetPosition = pointB.position;
    }

    void Update()
    {
        if (!qteActive) return;
        if (pointA == null || pointB == null || pointerTransform == null) return;

        // ??????? pointer ????? target
        pointerTransform.position = Vector3.MoveTowards(
            pointerTransform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // ????????????????????? A ???? B
        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
        }

        // ???????? (????????????? KeyCode.E ?????)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        if (safeZone == null || pointerTransform == null) return;

        bool inSafe = RectTransformUtility.RectangleContainsScreenPoint(
            safeZone,
            pointerTransform.position,
            null   // ??? Canvas ???? Screen Space - Overlay ??? null ???
        );

        if (inSafe)
        {
            Debug.Log("QTE Success!");
            failCount = 0;
        }
        else
        {
            failCount++;
            Debug.Log($"QTE Fail {failCount}/{maxFails}");

            if (failCount >= maxFails)
            {
                OnQTEFailedMax();
            }
        }
    }

    void OnQTEFailedMax()
    {
        Debug.Log("QTE failed 3 times -> Enemy starts chasing your hiding spot!");

        qteActive = false;

        // ??? UI QTE
        if (qteRoot != null)
            qteRoot.SetActive(false);

        // ?????????????? "??????????????" ??? player ????
        if (enemyAI != null)
        {
            enemyAI.CatchPlayerInHiding(hidingTarget);
        }

        // ????????????????
        enabled = false;
    }
}
