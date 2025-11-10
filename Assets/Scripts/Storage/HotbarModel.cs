using System;
using System.Collections;
using UnityEngine;

public class HotbarModel : MonoBehaviour
{
    public static HotbarModel Instance { get; private set; }

    [Range(1, 10)] public int hotbarSize = 5;
    public int[] mapInvIndex;      // -1 = ว่าง
    public int selected = 0;

    public event Action<int, ItemDefinition> OnSelectionChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Debug.Log(Instance == null);
        if (hotbarSize < 1) hotbarSize = 5;
        if (mapInvIndex == null || mapInvIndex.Length != hotbarSize)
        {
            mapInvIndex = new int[hotbarSize];
            for (int i = 0; i < hotbarSize; i++) mapInvIndex[i] = -1;
        }
    }

    void OnEnable()
    {
        // อย่า subscribe ทันที — รอ Inventory พร้อมก่อน
        StartCoroutine(BindWhenReady());
    }

    void OnDisable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.OnInventoryChanged -= AutoFill;
        OnSelectionChanged = null;
    }

    IEnumerator BindWhenReady()
    {
        float t = 0f;
        while (Inventory.Instance == null && t < 2f)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogWarning("[HotbarModel] Inventory.Instance is null (not in scene or created too late).");
            yield break;
        }

        Inventory.Instance.OnInventoryChanged += AutoFill;
        AutoFill(); // สร้างแม็ปครั้งแรก
        OnSelectionChanged?.Invoke(selected, GetSelectedItem()); // sync เริ่มต้น
    }

    public void SelectIndex(int idx)
    {
        if (hotbarSize < 1) return;
        selected = Mathf.Clamp(idx, 0, hotbarSize - 1);
        OnSelectionChanged?.Invoke(selected, GetSelectedItem());
    }

    public void SelectNextFilled(int dir)
    {
        if (hotbarSize < 1) return;
        for (int step = 0; step < hotbarSize; step++)
        {
            selected = (selected + (dir > 0 ? 1 : -1) + hotbarSize) % hotbarSize;
            if (GetSelectedItem() != null)
            {
                OnSelectionChanged?.Invoke(selected, GetSelectedItem());
                return;
            }
        }
        OnSelectionChanged?.Invoke(selected, null);
    }

    public ItemDefinition GetSelectedItem()
    {
        if (Inventory.Instance == null) return null;
        if (mapInvIndex == null || mapInvIndex.Length == 0) return null;

        int invIdx = mapInvIndex[Mathf.Clamp(selected, 0, hotbarSize - 1)];
        if (invIdx < 0) return null;

        var inv = Inventory.Instance;
        if (invIdx >= inv.slots.Count) return null;
        var s = inv.slots[invIdx];
        return s.IsEmpty ? null : s.item;
    }

    void AutoFill()
    {
        if (Inventory.Instance == null) return;

        var inv = Inventory.Instance;

        // เคลียร์ช่องที่ชี้ไปสลอตว่าง
        for (int i = 0; i < mapInvIndex.Length; i++)
        {
            int idx = mapInvIndex[i];
            if (idx >= 0 && (idx >= inv.slots.Count || inv.slots[idx].IsEmpty))
                mapInvIndex[i] = -1;
        }

        // เติมของที่ถือได้ (มี heldPrefab) ลงช่องว่าง
        for (int invIdx = 0; invIdx < inv.slots.Count; invIdx++)
        {
            var s = inv.slots[invIdx];
            if (s.IsEmpty || s.item.heldPrefab == null) continue;

            bool already = false;
            for (int h = 0; h < mapInvIndex.Length; h++)
                if (mapInvIndex[h] == invIdx) { already = true; break; }
            if (already) continue;

            for (int h = 0; h < mapInvIndex.Length; h++)
                if (mapInvIndex[h] == -1) { mapInvIndex[h] = invIdx; break; }
        }

        OnSelectionChanged?.Invoke(selected, GetSelectedItem());
    }
}
