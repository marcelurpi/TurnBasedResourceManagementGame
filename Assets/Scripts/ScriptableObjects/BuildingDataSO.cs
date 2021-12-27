using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingDataSO : ScriptableObject
{
    [SerializeField] private TileDataSO tile = null;
    [SerializeField] private int daysToConstruct = 0;
    [SerializeField] private ItemStackForSO cost = ItemStackForSO.none;

    public TileDataSO GetTile() => tile;
    public string GetName() => tile.GetName();
    public Color GetColor() => tile.GetColor();
    public int GetDaysToConstruct() => daysToConstruct;
    public ItemStackForSO GetCost() => cost;
}
