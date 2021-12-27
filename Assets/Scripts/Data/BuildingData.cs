using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData : IData
{
    [SerializeField] private string tileName = null;
    [SerializeField] private int daysToConstruct = 0;
    [SerializeField] private ItemStack cost = ItemStack.none;

    private TileData tile = new TileData();

    public TileData GetTile() => tile;
    public string GetName() => tileName;
    public Color GetColor() => tile.GetColor();
    public int GetDaysToConstruct() => daysToConstruct;
    public ItemStack GetCost() => cost;

    public void OnValidate(ConfigSO configSaver)
    {
        cost.OnValidate(configSaver, tileName, "Building");

        if(string.IsNullOrEmpty(tileName))
        {
            tile = null;
            if (ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Tile Name Is Null", false);
        }
        else
        {
            tile = System.Array.Find(configSaver.GetDatas<TileData>(), t => t.GetName() == tileName);
            if (ConfigMessage.Instance != null && tile == null) ConfigMessage.Instance.TrySetMessageText($"Tile { tileName } Not Found In Its Building", false);
        }
    }

    public BuildingData() { }

    public BuildingData(BuildingDataSO dataSO, ConfigSO configSaver)
    {
        tileName = dataSO.GetName();
        daysToConstruct = dataSO.GetDaysToConstruct();
        cost = new ItemStack(dataSO.GetCost(), configSaver, tileName, "Building");

        OnValidate(configSaver);
    }
}
