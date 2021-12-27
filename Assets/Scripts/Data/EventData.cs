using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData : IData
{
    [SerializeField] private string eventName = null;
    [SerializeField] private int daysToPrepare = 0;
    [SerializeField] private ItemStack cost = ItemStack.none;
    [SerializeField] private ItemStack reward = ItemStack.none;

    public string GetName() => eventName;
    public int GetDaysToPrepare() => daysToPrepare;
    public ItemStack GetCost() => cost;
    public ItemStack GetReward() => reward;

    public void OnValidate(ConfigSO configSaver)
    {
        if (string.IsNullOrEmpty(eventName) && ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Tile Name Is Null", false);

        cost.OnValidate(configSaver, eventName, "Event");
        reward.OnValidate(configSaver, eventName, "Event");
    }

    public EventData() { }

    public EventData(EventDataSO eventDataSO, ConfigSO currentConfig)
    {
        eventName = eventDataSO.GetName();
        daysToPrepare = eventDataSO.GetDaysToPrepare();

        cost = new ItemStack(eventDataSO.GetCost(), currentConfig, eventName, "Event");
        reward = new ItemStack(eventDataSO.GetReward(), currentConfig, eventName, "Event");
        
        OnValidate(currentConfig);
    }
}
