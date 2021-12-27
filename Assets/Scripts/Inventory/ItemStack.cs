using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemStack
{
    public enum Sign
    {
        None,
        Positive,
        Negative,
    }

    public static readonly ItemStack none = new ItemStack(null, 0);

    [SerializeField] private string itemName;
    [SerializeField] private int amount;

    private ItemData item;

    public string GetName() => itemName;
    public int GetAmount() => amount;

    public bool IsNone() => string.IsNullOrEmpty(itemName) || amount == 0;

    public void SetAmount(int amount)
    {
        if (amount < 0) throw new System.ArgumentOutOfRangeException("ItemStack cannot have negative amount");

        this.amount = amount;

        if (amount == 0)
        {
            item = null;
            itemName = null;
        }
    }

    public override string ToString()
    {
        return ToString(Sign.None);
    }

    public string ToString(Sign sign)
    {
        if (IsNone()) return "";

        switch (sign)
        {
            case Sign.None:
                return $"{ amount } { item.GetName() }";
            case Sign.Positive:
                return $"+{ amount } { item.GetName() }";
            case Sign.Negative:
                return $"-{ amount } { item.GetName() }";
            default:
                throw new System.ArgumentOutOfRangeException("ItemStack ToString with a sign not implemented");
        }
    }

    private ItemStack(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;

        itemName = item?.GetName() ?? null;
    }

    public ItemStack(ItemStackForSO itemStack, ConfigSO configSaver, string parentName, string parentTypeName)
    {
        itemName = itemStack.GetName();
        amount = itemStack.GetAmount();

        item = new ItemData();

        OnValidate(configSaver, parentName, parentTypeName);
    }

    public void OnValidate(ConfigSO configSaver, string parentName, string parentTypeName)
    {
        if (amount == 0) return;

        if (string.IsNullOrEmpty(itemName))
        {
            item = null;
            if (ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Item Name Is Null", false);
        }
        else
        {
            string itemName = this.itemName;
            item = System.Array.Find(configSaver.GetDatas<ItemData>(), i => i.GetName() == itemName);
            if (ConfigMessage.Instance != null && item == null)
            {
                ConfigMessage.Instance.TrySetMessageText($"Item { itemName } Not Found In { parentTypeName } { parentName }", false);
                this.itemName = null;
            }
        }
    }
}
