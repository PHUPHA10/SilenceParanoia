using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [Header("Pickup Info")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData;   // เชื่อม ScriptableObject ของไอเท็ม
    [SerializeField] private int amount = 1;

    public string Prompt =>

        "เก็บ " + (string.IsNullOrEmpty(itemName) ?
        itemData?.displayName :
        itemName);

    public void Interact()
    {
        if (itemData == null)
        {
            Debug.LogWarning($"เก็บ '{gameObject.name}' has no ItemDefinition assigned!");
            Destroy(gameObject);
            return;
        }

        // เพิ่มเข้ากระเป๋า
        int added = Inventory.Instance?.Add(itemData, amount) ?? 0;
        if (added > 0)
        {
            Debug.Log($"เก็บ {added}x {itemData.displayName}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full or cannot pick up item.");
        }
    }
}
