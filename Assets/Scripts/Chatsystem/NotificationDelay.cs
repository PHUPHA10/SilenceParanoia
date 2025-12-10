using StarterAssets;
using UnityEngine;
using UnityEngine.UI;   // ?????????????????

public class NotificationDelay : MonoBehaviour
{
    [Header("Notification UI")]
    public RectTransform notificationUI;
    public float delayTime = 30f;
    public float moveSpeed = 300f;

    [Header("Move Points")]
    public RectTransform pointA;
    public RectTransform pointB;

    [Header("Chat System")]
    public GameObject chatPanel;            // <- ????????????????!

    private bool startMove = false;

    void Start()
    {
        if (notificationUI != null)
        {
            notificationUI.anchoredPosition = pointA.anchoredPosition;
            notificationUI.gameObject.SetActive(false);
        }

        if (chatPanel != null)
            chatPanel.SetActive(false);     // ?????????????

        // ????????????
        Invoke(nameof(ShowAndMove), delayTime);

        // ????????????????
        Button btn = notificationUI.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OpenChat);  // ???????????????????
        }
    }

    void ShowAndMove()
    {
        if (notificationUI != null)
        {
            notificationUI.gameObject.SetActive(true);
            startMove = true;   // ?????????????
        }

        // ? ???????????????????????????????? Notification ???
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void Update()
    {
        if (!startMove) return;

        notificationUI.anchoredPosition = Vector2.MoveTowards(
            notificationUI.anchoredPosition,
            pointB.anchoredPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(notificationUI.anchoredPosition, pointB.anchoredPosition) < 0.1f)
        {
            startMove = false;
        }
    }

    public void OpenChat()
    {
        Debug.Log("Opening Chat from Notification");

        // 1) ???? Notification
        if (notificationUI != null)
            notificationUI.gameObject.SetActive(false);

        // 2) ???????????
        if (chatPanel != null)
            chatPanel.SetActive(true);

        // 3) ??????????????????????????? (????????? ????????)
        var fpc = FindObjectOfType<FirstPersonController>();
        if (fpc != null) fpc.enabled = false;

        var playerInteract = FindObjectOfType<PlayerInteract>();
        if (playerInteract != null) playerInteract.enabled = false;

        // 4) ????????????????? UI ???
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
