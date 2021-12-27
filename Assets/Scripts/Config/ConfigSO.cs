using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ConfigSO : ScriptableObject
{
    [SerializeField] private string currentMissionName = null;
    [SerializeField] private BuildingData[] buildings = null;
    [SerializeField] private EventData[] events = null;
    [SerializeField] private ItemData[] items = null;
    [SerializeField] private MissionData[] missions = null;
    [SerializeField] private TileData[] tiles = null;

    private TileData emptyTile = null;
    private MissionData currentMission = null;

    public TileData GetEmptyTile() => emptyTile;
    public MissionData GetCurrentMission() => currentMission;

    public void OnValidate()
    {
        ApplyOnValidate(buildings);
        ApplyOnValidate(events);
        ApplyOnValidate(items);
        ApplyOnValidate(missions);
        ApplyOnValidate(tiles);

        UpdateEmptyTile();
        UpdateCurrentMission();
    }

    public void LoadFromScriptableObjects()
    {
        LoadToArray<BuildingDataSO, BuildingData>(ref buildings, scriptableObject => new BuildingData(scriptableObject, this));
        LoadToArray<EventDataSO, EventData>(ref events, scriptableObject => new EventData(scriptableObject, this));
        LoadToArray<ItemDataSO, ItemData>(ref items, scriptableObject => new ItemData(scriptableObject, this));
        LoadToArray<MissionDataSO, MissionData>(ref missions, scriptableObject => new MissionData(scriptableObject, this));
        LoadToArray<TileDataSO, TileData>(ref tiles, scriptableObject => new TileData(scriptableObject, this));

        OnValidate();
    }

    public void LoadConfigContentsFrom(ConfigSO config)
    {
        currentMissionName = config.currentMissionName;
        buildings = config.buildings;
        events = config.events;
        items = config.items;
        missions = config.missions;
        tiles = config.tiles;

        OnValidate();
    }

    public void SetCurrentMission(string currentMissionName)
    {
        this.currentMissionName = currentMissionName;
        UpdateCurrentMission();
    }

    public T[] GetDatas<T>() where T : IData
    {
        switch (typeof(T).Name)
        {
            case "BuildingData":    return buildings.Cast<T>().ToArray();
            case "EventData":       return events.Cast<T>().ToArray();
            case "ItemData":        return items.Cast<T>().ToArray();
            case "MissionData":     return missions.Cast<T>().ToArray();
            case "TileData":        return tiles.Cast<T>().ToArray();
            default: return null;
        }
    }

    public void SetDatas<T>(T[] newDatas) where T : IData
    {
        switch (typeof(T).Name)
        {
            case "BuildingData":  buildings = newDatas.Cast<BuildingData>().ToArray();  ApplyOnValidate(buildings); break;
            case "EventData":     events = newDatas.Cast<EventData>().ToArray();        ApplyOnValidate(events);    break;
            case "ItemData":      items = newDatas.Cast<ItemData>().ToArray();          ApplyOnValidate(items);     break;
            case "MissionData":   missions = newDatas.Cast<MissionData>().ToArray();    ApplyOnValidate(missions);  break;
            case "TileData":      tiles = newDatas.Cast<TileData>().ToArray();          ApplyOnValidate(tiles);     break;
            default: break;
        }

        OnValidate();
    }

    public void AddElementToData<T>()
    {
        switch (typeof(T).Name)
        {
            case "BuildingData":    AddElementToArray(ref buildings);  ApplyOnValidate(buildings); break;
            case "EventData":       AddElementToArray(ref events);     ApplyOnValidate(events);    break;
            case "ItemData":        AddElementToArray(ref items);      ApplyOnValidate(items);     break;
            case "MissionData":     AddElementToArray(ref missions);   ApplyOnValidate(missions);  break;
            case "TileData":        AddElementToArray(ref tiles);      ApplyOnValidate(tiles);     break;
            default: break;
        }

        OnValidate();
    }

    private void ApplyOnValidate(IData[] datas)
    {
        foreach (IData data in datas)
        {
            data.OnValidate(this);
        }
    }

    private void UpdateEmptyTile()
    {
        emptyTile = System.Array.Find(tiles, t => t.GetName().ToLower() == "empty");
        if (ConfigMessage.Instance != null && emptyTile == null) ConfigMessage.Instance.TrySetMessageText("Empty Tile Not Found", false);
    }

    private void UpdateCurrentMission()
    {
        if (string.IsNullOrEmpty(currentMissionName))
        {
            currentMission = null;
            if (ConfigMessage.Instance != null) ConfigMessage.Instance.TrySetMessageText($"Current Mission Name Is Null", false);
        }
        else
        {
            currentMission = System.Array.Find(missions, m => m.GetName() == currentMissionName);
            if (ConfigMessage.Instance != null && currentMission == null) ConfigMessage.Instance.TrySetMessageText($"Current Mission { currentMissionName } Not Found", false);
        }
    }

    private void AddElementToArray<T>(ref T[] array) where T : IData, new()
    {
        array = new List<T>(array) { new T() }.ToArray();
    }

    private void LoadToArray<TSO, T>(ref T[] array, System.Func<TSO, T> createInstance) where TSO : ScriptableObject
    {
        if (array == null) array = new T[0];

        string typeName = array.GetType().GetElementType().Name;
        TSO[] arraySOs = Resources.LoadAll($"{ typeName.Substring(0, typeName.Length - 4) }s").Cast<TSO>().ToArray();
        array = new T[arraySOs.Length];
        for (int i = 0; i < arraySOs.Length; i++)
        {
            array[i] = createInstance(arraySOs[i]);
        }
    }
}
