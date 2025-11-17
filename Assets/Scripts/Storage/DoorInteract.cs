using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [Header("Door Info")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData;   // เชื่อม ScriptableObject ของไอเท็ม
    [SerializeField] private Animator Door;

    private bool IsOpen;
    public string Prompt => "Pick Up " + (string.IsNullOrEmpty(itemName) ? itemData?.displayName : itemName);

    public void Interact()
    {
        if (itemData == null)
        {
            Debug.LogWarning($"Pickup '{gameObject.name}' has no ItemDefinition assigned!");
            Destroy(gameObject);
            return;
        }
        if (IsOpen)
        {
            Door.SetTrigger("Close");
            IsOpen = false;
        }
        else
        {
            Door.SetTrigger("Open");
            IsOpen = true;
        }
    }
}
