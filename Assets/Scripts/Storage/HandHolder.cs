using UnityEngine;

public class HandHolder : MonoBehaviour
{
    public Transform rightHandAnchor;    // จุดยึด (ลูกของกล้อง/แขนผู้เล่น)
    private GameObject currentHeld;

    void OnEnable()
    {
        HotbarModel.Instance.OnSelectionChanged += OnSelectChanged;
        Inventory.Instance.OnInventoryChanged += OnInventoryChanged;
        // sync ครั้งแรก
        OnSelectChanged(HotbarModel.Instance.selected, HotbarModel.Instance.GetSelectedItem());
    }

    void OnDisable()
    {
        if (HotbarModel.Instance != null)
            HotbarModel.Instance.OnSelectionChanged -= OnSelectChanged;
        if (Inventory.Instance != null)
            Inventory.Instance.OnInventoryChanged -= OnInventoryChanged;
    }

    void OnInventoryChanged()
    {
        // ถ้าของในช่องที่เลือกหาย ให้เคลียร์
        OnSelectChanged(HotbarModel.Instance.selected, HotbarModel.Instance.GetSelectedItem());
    }

    void OnSelectChanged(int idx, ItemDefinition item)
    {
        if (currentHeld) Destroy(currentHeld);
        if (item != null && item.heldPrefab != null)
        {
            currentHeld = Instantiate(item.heldPrefab, rightHandAnchor);
            currentHeld.transform.localPosition = Vector3.zero;
            currentHeld.transform.localRotation = Quaternion.identity;
        }
    }
    public void EquipItem(ItemDefinition def)
{
    if (rightHandAnchor == null) return;
    if (currentHeld) Destroy(currentHeld);

    if (def == null || def.heldPrefab == null) return;

    currentHeld = Instantiate(def.heldPrefab, rightHandAnchor);
    currentHeld.transform.localPosition = def.heldLocalPosition;
    currentHeld.transform.localRotation = Quaternion.Euler(def.heldLocalEuler);
    currentHeld.transform.localScale    = def.heldLocalScale;
}

}
