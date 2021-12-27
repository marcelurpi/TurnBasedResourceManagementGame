using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemStackForSO
{
    public static readonly ItemStackForSO none = new ItemStackForSO(null, 0);

    [SerializeField] private ItemDataSO item;
    [SerializeField] private int amount;

    public string GetName()
    {
        if (item == null) return null;
        return item.GetName();
    }

    public int GetAmount()
    {
        return amount;
    }

    private ItemStackForSO(ItemDataSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
