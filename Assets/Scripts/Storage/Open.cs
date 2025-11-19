using UnityEngine;

public class Open : MonoBehaviour, IInteractable
{
    [Header("Pickup Info")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData;   // เชื่อม ScriptableObject ของไอเท็ม
    [SerializeField] private int amount = 1;

    public string Prompt =>

        "Open " + (string.IsNullOrEmpty(itemName) ?
        itemData?.displayName :
        itemName);


    public void Interact()
    {
        
    }
}
