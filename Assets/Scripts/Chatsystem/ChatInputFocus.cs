using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChatInputFocus : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public static bool IsAnyInputFocused { get; private set; }

    public void OnSelect(BaseEventData eventData)
    {
        IsAnyInputFocused = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        IsAnyInputFocused = false;
    }
}
