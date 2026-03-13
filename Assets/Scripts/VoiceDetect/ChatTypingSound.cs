using UnityEngine;
using TMPro;

public class ChatTypingSound : MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField inputField;

    [Header("Audio")]
    public AudioSource audioSource;     // AudioSource กลาง (2D)
    public AudioClip typingSound;       // เสียงกดคีย์บอร์ด

    private int lastTextLength = 0;

    void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(OnTextChanged);
        }
    }

    void OnTextChanged(string newText)
    {
        // พิมพ์เพิ่ม = เล่นเสียง
        if (newText.Length > lastTextLength)
        {
            PlayTypingSound();
        }

        lastTextLength = newText.Length;
    }

    void PlayTypingSound()
    {
        if (audioSource != null && typingSound != null)
        {
            audioSource.PlayOneShot(typingSound);
        }
    }
}
