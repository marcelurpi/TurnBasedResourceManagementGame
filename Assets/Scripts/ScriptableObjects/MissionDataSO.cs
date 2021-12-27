using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MissionDataSO : ScriptableObject
{
    [SerializeField] private string missionName = null;
    [SerializeField] private ItemStackForSO goal = ItemStackForSO.none;
    [SerializeField] private int maxDaysToComplete = 0;
    [SerializeField] private TileDataSO[] startingTiles = null;
    [SerializeField] private BuildingDataSO[] alreadyPlacedBuildings = null;
    [SerializeField] private BuildingDataSO[] allowedBuildings = null;
    [SerializeField] private EventDataSO[] allowedEvents = null;

    public string GetName() => missionName;
    public ItemStackForSO GetGoal() => goal;
    public int GetMaxDaysToComplete() => maxDaysToComplete;
    public TileDataSO[] GetStartingTiles() => startingTiles;
    public BuildingDataSO[] GetAlreadyPlacedBuildings() => alreadyPlacedBuildings;
    public BuildingDataSO[] GetAllowedBuildings() => allowedBuildings;
    public EventDataSO[] GetAllowedEvents() => allowedEvents;
}
