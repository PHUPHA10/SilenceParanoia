using UnityEngine;

public class ChatSwitcher : MonoBehaviour
{
    [Header("Chat Panels")]
    public GameObject[] chatPanels;

    [Header("Profile Buttons")]
    public RectTransform[] profileButtons;

    [Header("Audio")]
    public AudioSource chatAudio;     // AudioSource กลาง (อยู่ใต้ Canvas)
    public AudioClip switchClip;      // เสียงตอนสลับแชท

    int currentIndex = 0;

    void Start()
    {
        OpenChat(0);
    }

    public void OpenChat(int index)
    {
        if (index < 0 || index >= chatPanels.Length)
            return;

        // 🔊 เล่นเสียงเฉพาะตอนสลับจริง
        if (index != currentIndex)
        {
            if (chatAudio != null && switchClip != null)
                chatAudio.PlayOneShot(switchClip);

            SwapProfilePositions(currentIndex, index);
        }

        // เปิดเฉพาะแชทที่เลือก
        for (int i = 0; i < chatPanels.Length; i++)
        {
            if (chatPanels[i] != null)
                chatPanels[i].SetActive(i == index);
        }

        currentIndex = index;
    }

    void SwapProfilePositions(int oldIndex, int newIndex)
    {
        if (profileButtons == null ||
            oldIndex >= profileButtons.Length ||
            newIndex >= profileButtons.Length)
            return;

        Vector3 temp = profileButtons[oldIndex].anchoredPosition;
        profileButtons[oldIndex].anchoredPosition =
            profileButtons[newIndex].anchoredPosition;
        profileButtons[newIndex].anchoredPosition = temp;
    }
}
