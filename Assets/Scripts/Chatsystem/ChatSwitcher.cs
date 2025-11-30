using UnityEngine;

public class ChatSwitcher : MonoBehaviour
{
    public GameObject[] chatPanels;   // ??? Panel ???????????????

    public void OpenChat(int index)
    {
        // ??????????????
        for (int i = 0; i < chatPanels.Length; i++)
        {
            if (chatPanels[i] != null)
                chatPanels[i].SetActive(i == index);
        }
    }
}
