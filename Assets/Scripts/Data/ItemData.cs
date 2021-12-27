using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData : IData
{
    [SerializeField] private string itemName = null;

    public string GetName() => itemName;

    public void OnValidate(ConfigSO configSaver) 
    {
        if (string.IsNullOrEmpty(itemName) && ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Item Name Is Null", false);
    }

    public ItemData() { }

    public ItemData(ItemDataSO dataSO, ConfigSO configSaver)
    {
        itemName = dataSO.GetName();
    }
}
