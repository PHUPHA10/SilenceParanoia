using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationDelay : MonoBehaviour
{
    [Header("Notification UI")]
    public RectTransform notificationUI;
    public float delayTime = 4f;
    public float moveSpeed = 300f;

    [Header("Move Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Chat System")]
    public GameObject chatPanel;
    public TMP_Text helpText;

    [Header("Audio")]
    public AudioSource notificationAudio;   // AudioSource กลาง (อยู่ใต้ Canvas)
    public AudioClip popupSound;             // 🔔 เสียงแจ้งเตือนเด้ง
    public AudioClip clickSound;             // 👆 เสียงกดแจ้งเตือน
    public float soundLeadTime = 0.1f;        // เสียงเด้งมาก่อน UI

    bool movingDown;
    bool clicked;
    bool consumed;

    void Start()
    {
        if (notificationUI != null && pointA != null)
        {
            notificationUI.position = pointA.position;
            notificationUI.gameObject.SetActive(false);
        }

        if (chatPanel != null)
            chatPanel.SetActive(false);


        Invoke(nameof(PlayPopupSound),
            Mathf.Max(0f, delayTime - soundLeadTime));


        Invoke(nameof(ShowNotification), delayTime);


        if (notificationUI != null)
        {
            Button btn = notificationUI.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(OnNotificationClicked);
        }
    }

    void PlayPopupSound()
    {
        if (notificationAudio != null && popupSound != null)
            notificationAudio.PlayOneShot(popupSound);
    }

    void ShowNotification()
    {
        if (notificationUI == null || pointB == null || consumed)
            return;

        notificationUI.gameObject.SetActive(true);
        movingDown = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (notificationUI == null || consumed) return;

        if (movingDown)
        {
            notificationUI.position = Vector3.MoveTowards(
                notificationUI.position,
                pointB.position,
                moveSpeed * Time.deltaTime);

            if (Vector3.Distance(notificationUI.position, pointB.position) < 0.1f)
                movingDown = false;
        }

        // กด M = ถือว่า "กดแจ้งเตือน"
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnNotificationClicked();
        }
    }

    void OnNotificationClicked()
    {
        if (consumed) return;
        consumed = true;

        // 👆 เล่นเสียงกด
        if (notificationAudio != null && clickSound != null)
            notificationAudio.PlayOneShot(clickSound);

        if (notificationUI != null)
            notificationUI.gameObject.SetActive(false);

        if (helpText != null)
            helpText.gameObject.SetActive(false);

        if (chatPanel != null)
            chatPanel.SetActive(true);
    }
}
