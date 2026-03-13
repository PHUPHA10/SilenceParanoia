using UnityEngine;

public class close : MonoBehaviour, IInteractable
{
    [Header("ปิด")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData;   // เชื่อม ScriptableObject ของไอเท็ม
    [SerializeField] private int amount = 0;

    public string Prompt =>

        "ปิด " + (string.IsNullOrEmpty(itemName) ?
        itemData?.displayName :
        itemName);


    public void Interact()
    {

    }
}
