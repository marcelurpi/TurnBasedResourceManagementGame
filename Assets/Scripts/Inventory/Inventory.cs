using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour, IInspectorSetter
{
    public static Inventory Instance { get; private set; }

    public event System.Action OnInventoryContentsUpdated;

    [SerializeField] private InventorySlot[] slots = null;

    private ItemStack[] contents;

    private void Awake()
    {
        Instance = this;
    }

    public void Construct()
    {
        contents = new ItemStack[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            contents[i] = ItemStack.none;
            slots[i].SetItem(ItemStack.none);
        }
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectsInChildByType(transform, ref slots);
    }

    public void Store(ItemStack toStore)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].GetName() == toStore.GetName())
            {
                contents[i].SetAmount(contents[i].GetAmount() + toStore.GetAmount());
                slots[i].SetItem(contents[i]);
                OnInventoryContentsUpdated?.Invoke();
                return;
            }
        }
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].IsNone())
            {
                contents[i] = toStore;
                slots[i].SetItem(contents[i]);
                OnInventoryContentsUpdated?.Invoke();
                return;
            }
        }
    }

    public bool Contains(ItemStack toCheck)
    {
        if (toCheck.IsNone()) return true;

        foreach (ItemStack item in contents)
        {
            if (item.GetName() == toCheck.GetName() && item.GetAmount() >= toCheck.GetAmount())
            {
                return true;
            }
        }
        return false;
    }

    public bool HasSpaceFor(ItemStack toCheck)
    {
        if (toCheck.IsNone()) return true;

        foreach (ItemStack item in contents)
        {
            if(item.GetName() == toCheck.GetName() || item.IsNone())
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemAmountStored(string itemName)
    {
        foreach (ItemStack stored in contents)
        {
            if(stored.GetName() == itemName)
            {
                return stored.GetAmount();
            }
        }
        return 0;
    }

    public void Retrieve(ItemStack toRetrieve)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].GetName() == toRetrieve.GetName())
            {
                contents[i].SetAmount(Mathf.Max(0, contents[i].GetAmount() - toRetrieve.GetAmount()));

                slots[i].SetItem(contents[i]);
                OnInventoryContentsUpdated?.Invoke();
                return;
            }
        }
    }

    public void RemoveItemInSlot(int slotIndex)
    {
        contents[slotIndex] = ItemStack.none;
        slots[slotIndex].SetItem(contents[slotIndex]);
        OnInventoryContentsUpdated?.Invoke();
    }
}
