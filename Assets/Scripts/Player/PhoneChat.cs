using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneChat : MonoBehaviour
{
    [Header("UI")]
    public GameObject phoneChatRoot;

    [Header("Disable these when chat is open")]
    public MonoBehaviour[] componentsToDisable;

    bool isOpen = false;

    void Start()
    {
        phoneChatRoot.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleChat();
        }
    }

    void ToggleChat()
    {
        isOpen = !isOpen;
        phoneChatRoot.SetActive(isOpen);

        foreach (var comp in componentsToDisable)
        {
            if (comp != null)
                comp.enabled = !isOpen;
        }

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }
}
