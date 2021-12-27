using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class MissionData : IData
{
    [SerializeField] private string missionName = null;
    [SerializeField] private ItemStack goal = ItemStack.none;
    [SerializeField] private int maxDaysToComplete = 0;
    [SerializeField] private string[] startingTilesNames = null;
    [SerializeField] private string[] alreadyPlacedBuildingsNames = null;
    [SerializeField] private string[] allowedBuildingsNames = null;
    [SerializeField] private string[] allowedEventNames = null;

    private TileData[] startingTiles = null;
    private BuildingData[] alreadyPlacedBuildings = null;
    private BuildingData[] allowedBuildings = null;
    private EventData[] allowedEvents = null;

    public string GetName() => missionName;
    public ItemStack GetGoal() => goal;
    public int GetMaxDaysToComplete() => maxDaysToComplete;
    public TileData[] GetStartingTiles() => startingTiles;
    public BuildingData[] GetAlreadyPlacedBuildings() => alreadyPlacedBuildings;
    public BuildingData[] GetAllowedBuildings() => allowedBuildings;
    public EventData[] GetAllowedEvents() => allowedEvents;

    public void OnValidate(ConfigSO configSaver)
    {
        if (string.IsNullOrEmpty(missionName) && ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Mission Name Is Null", false);

        goal.OnValidate(configSaver, missionName, "Mission");

        OnValidateArray(configSaver.GetDatas<TileData>(), ref startingTiles, ref startingTilesNames);

        BuildingData[] buildings = configSaver.GetDatas<BuildingData>();
        OnValidateArray(buildings, ref alreadyPlacedBuildings, ref alreadyPlacedBuildingsNames);
        OnValidateArray(buildings, ref allowedBuildings, ref allowedBuildingsNames);

        OnValidateArray(configSaver.GetDatas<EventData>(), ref allowedEvents, ref allowedEventNames);
    }

    public void OnValidateArray<T>(T[] arrayFind, ref T[] array, ref string[] arrayNames) where T : IData
    {
        if (arrayNames == null) arrayNames = new string[0];

        if (array == null || array.Length != arrayNames.Length) array = new T[arrayNames.Length];

        for (int i = 0; i < arrayNames.Length; i++)
        {
            string typeName = typeof(T).Name.Substring(0, typeof(T).Name.Length - 4);
            if (string.IsNullOrEmpty(arrayNames[i]))
            {
                array[i] = default;
                if (ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"{ typeName } Name Is Null", false);
            }
            else
            {
                string elementName = arrayNames[i];
                array[i] = System.Array.Find(arrayFind, e => e.GetName() == elementName);
                if (ConfigMessage.Instance != null && array[i] == null)
                {
                    ConfigMessage.Instance.TrySetMessageText($"{ typeName } { elementName } Not Found In Mission { missionName }", false);
                }
            }
        }
    }

    public MissionData() { }

    public MissionData(MissionDataSO dataSO, ConfigSO configSaver)
    {
        missionName = dataSO.GetName();
        goal = new ItemStack(dataSO.GetGoal(), configSaver, missionName, "Mission");
        maxDaysToComplete = dataSO.GetMaxDaysToComplete();

        startingTilesNames = dataSO.GetStartingTiles().Select(t => t.GetName()).ToArray();
        alreadyPlacedBuildingsNames = dataSO.GetAlreadyPlacedBuildings().Select(b => b.GetName()).ToArray();
        allowedBuildingsNames = dataSO.GetAllowedBuildings().Select(b => b.GetName()).ToArray();
        allowedEventNames = dataSO.GetAllowedEvents().Select(e => e.GetName()).ToArray();

        OnValidate(configSaver);
    }
}
