using UnityEngine;

public class Open : MonoBehaviour, IInteractable
{
    [Header("เปิด")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData;   // เชื่อม ScriptableObject ของไอเท็ม
    [SerializeField] private int amount = 0;

    public string Prompt =>

        "เปิด " + (string.IsNullOrEmpty(itemName) ?
        itemData?.displayName :
        itemName);


    public void Interact()
    {
        
    }
}
