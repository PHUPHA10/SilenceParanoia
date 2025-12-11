using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class NotificationDelay : MonoBehaviour
{
    [Header("Notification UI")]
    public RectTransform notificationUI;
    public float delayTime = 30f;
    public float moveSpeed = 300f;

    [Header("Move Points")]
    public Transform pointA;   // ???????
    public Transform pointB;   // ???????

    [Header("Chat System")]
    public GameObject chatPanel;

    private bool movingDown = false;
    private bool movingUp = false;
    private bool clicked = false;

    private float stayTimer = 0f;
    private float stayDuration = 10f;  // ??????????? 15 ??????

    void Start()
    {
        if (notificationUI != null && pointA != null)
        {
            notificationUI.position = pointA.position;
            notificationUI.gameObject.SetActive(false);
        }

        if (chatPanel != null)
            chatPanel.SetActive(false);

        // ??????????????????????
        Invoke(nameof(ShowAndMoveDown), delayTime);

        // ?????????????? ? ???????
        if (notificationUI != null)
        {
            Button btn = notificationUI.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(OpenChat);
        }
    }

    void ShowAndMoveDown()
    {
        if (notificationUI == null || pointB == null) return;

        notificationUI.gameObject.SetActive(true);
        movingDown = true;
        movingUp = false;
        clicked = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (notificationUI == null || pointA == null || pointB == null) return;

        if (movingDown)
        {
            MoveTo(pointB.position);

            // ?????????? ? ???????????? 15 ??
            if (IsNear(notificationUI.position, pointB.position))
            {
                movingDown = false;
                stayTimer = 0f;
            }
        }
        else if (!clicked && notificationUI.gameObject.activeSelf)
        {
            // ?????????????? ?
            stayTimer += Time.deltaTime;

            if (stayTimer >= stayDuration)
            {
                movingUp = true;   // ??? 15 ?? ? ??????????????
            }
        }

        if (movingUp)
        {
            MoveTo(pointA.position);

            if (IsNear(notificationUI.position, pointA.position))
            {
                movingUp = false;
                notificationUI.gameObject.SetActive(false);
            }
        }
    }

    private void MoveTo(Vector3 targetPos)
    {
        notificationUI.position =
            Vector3.MoveTowards(
                notificationUI.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
    }

    private bool IsNear(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) < 0.1f;
    }

    public void OpenChat()
    {
        clicked = true;

        if (notificationUI != null)
            notificationUI.gameObject.SetActive(false);

        if (chatPanel != null)
            chatPanel.SetActive(true);

        var fpc = FindObjectOfType<FirstPersonController>();
        if (fpc != null) fpc.enabled = false;

        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null) playerInteract.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
