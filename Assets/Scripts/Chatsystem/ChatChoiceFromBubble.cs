using UnityEngine;
using TMPro;

public class ChatChoiceFromBubble : MonoBehaviour
{
    [Header("Choice")]
    public ChatChoiceFromBubble otherChoice;   // ช้อยส์อีกอัน

    [Header("Result Bubble (Right Side)")]
    public GameObject playerBubble;            // กล่องสีฟ้าฝั่งขวา
    public TMP_Text playerBubbleText;          // Text ในกล่องฟ้า
    public TMP_Text choiceText;                // Text ของช้อยส์นี้
    public GameObject timebubble;

    [Header("Audio")]
    public AudioSource chatAudio;              // AudioSource กลาง
    public AudioClip greenChoiceSound;         // เสียงช้อยส์เขียว
    public AudioClip redChoiceSound;           // เสียงช้อยส์แดง

    [Header("Choice Type")]
    public bool isGreenChoice = true;           // ✔ เขียว / ❌ แดง

    bool used = false;

    void Update()
    {
        if (used) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryClick();
        }
    }

    void TryClick()
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (!RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            Input.mousePosition,
            null))
            return;

        OnSelected();
    }

    void OnSelected()
    {
        used = true;

        // 🔊 เล่นเสียงตามช้อยส์
        PlayChoiceSound();

        // ❌ ลบอีกช้อยส์
        if (otherChoice != null)
            Destroy(otherChoice.gameObject);

        // ✅ แสดงกล่องฟ้าฝั่งขวา
        if (playerBubble != null && playerBubbleText != null)
        {
            playerBubbleText.text = choiceText.text;
            playerBubble.SetActive(true);
            timebubble.SetActive(true);
        }

        // ❌ ลบตัวเอง (ช้อยส์ที่เลือก)
        Destroy(gameObject);
    }

    void PlayChoiceSound()
    {
        if (chatAudio == null) return;

        if (isGreenChoice && greenChoiceSound != null)
        {
            chatAudio.PlayOneShot(greenChoiceSound);
        }
        else if (!isGreenChoice && redChoiceSound != null)
        {
            chatAudio.PlayOneShot(redChoiceSound);
        }
    }
}
