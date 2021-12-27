using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData : IData
{
    [SerializeField] private string tileName = null;
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private int peopleCapacity = 0;
    [SerializeField] private TileAction action = null;
    [SerializeField] private Color color = Color.white;

    public string GetName() => tileName;
    public bool IsBuilding() => isBuilding;
    public int GetPeopleCapacity() => peopleCapacity;
    public string GetActionName() => action.GetName();
    public ItemStack GetActionPrimaryCost() => action.GetPrimaryCost();
    public ItemStack GetActionSecondaryCost() => action.GetSecondaryCost();
    public ItemStack GetActionReward() => action.GetReward();
    public Color GetColor() => color;

    public void OnValidate(ConfigSO configSaver)
    {
        if (string.IsNullOrEmpty(tileName) && ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Tile Name Is Null", false);

        if (action == null) return;

        action.OnValidate(configSaver, tileName);
    }

    public TileData() {}

    public TileData(TileDataSO dataSO, ConfigSO configSaver)
    {
        tileName = dataSO.GetName();
        isBuilding = dataSO.IsBuilding();
        peopleCapacity = dataSO.GetPeopleCapacity();
        action = new TileAction(dataSO.GetActionName(),
                                new ItemStack(dataSO.GetActionPrimaryCost(), configSaver, tileName, "Tile"),
                                new ItemStack(dataSO.GetActionSecondaryCost(), configSaver, tileName, "Tile"),
                                new ItemStack(dataSO.GetActionReward(), configSaver, tileName, "Tile"));
        color = dataSO.GetColor();

        OnValidate(configSaver);
    }

    [System.Serializable]
    private class TileAction
    {
        [SerializeField] private string actionName = null;
        [SerializeField] private ItemStack primaryCost = ItemStack.none;
        [SerializeField] private ItemStack secondaryCost = ItemStack.none;
        [SerializeField] private ItemStack reward = ItemStack.none;

        public string GetName() => actionName;
        public ItemStack GetPrimaryCost() => primaryCost;
        public ItemStack GetSecondaryCost() => secondaryCost;
        public ItemStack GetReward() => reward;

        public TileAction(string actionName, ItemStack primaryCost, ItemStack secondaryCost, ItemStack reward)
        {
            this.actionName = actionName;
            this.primaryCost = primaryCost;
            this.secondaryCost = secondaryCost;
            this.reward = reward;
        }

        public void OnValidate(ConfigSO configSaver, string tileName)
        {
            primaryCost.OnValidate(configSaver, tileName, "Tile");
            secondaryCost.OnValidate(configSaver, tileName, "Tile");
            reward.OnValidate(configSaver, tileName, "Tile");
        }
    }
}
