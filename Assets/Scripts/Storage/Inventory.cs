using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Serializable]
    public class Slot
    {
        public ItemDefinition item;
        public int count;
        public bool IsEmpty => item == null || count <= 0;
        public bool CanStack(ItemDefinition def) => !IsEmpty && item == def && count < item.maxStack;
    }

    [SerializeField] private int capacity = 20;
    public List<Slot> slots = new List<Slot>();

    public event Action OnInventoryChanged;
    public event Action<ItemDefinition,int> OnItemPicked;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        for (int i = 0; i < capacity; i++) slots.Add(new Slot());
    }

    public int Add(ItemDefinition def, int amount)
    {
        if (def == null || amount <= 0) return 0;
        int remaining = amount;

        // เติมลงสแตกเดิม
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var s = slots[i];
            if (s.CanStack(def))
            {
                int canPut = Mathf.Min(def.maxStack - s.count, remaining);
                s.count += canPut;
                remaining -= canPut;
            }
        }
        // หา slot ว่าง
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var s = slots[i];
            if (s.IsEmpty)
            {
                int put = Mathf.Min(def.maxStack, remaining);
                s.item = def;
                s.count = put;
                remaining -= put;
            }
        }

        int added = amount - remaining;
        if (added > 0)
        {
            OnInventoryChanged?.Invoke();
            OnItemPicked?.Invoke(def, added);
        }
        return added;
    }

    public bool RemoveAt(int index, int amount = 1)
    {
        if (index < 0 || index >= slots.Count) return false;
        var s = slots[index];
        if (s.IsEmpty || amount <= 0) return false;
        s.count -= amount;
        if (s.count <= 0) { s.item = null; s.count = 0; }
        OnInventoryChanged?.Invoke();
        return true;
    }
}
