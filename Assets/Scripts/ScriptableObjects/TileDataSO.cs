using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileDataSO : ScriptableObject
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
    public ItemStackForSO GetActionPrimaryCost() => action.GetPrimaryCost();
    public ItemStackForSO GetActionSecondaryCost() => action.GetSecondaryCost();
    public ItemStackForSO GetActionReward() => action.GetReward();
    public Color GetColor() => color;

    [System.Serializable]
    private class TileAction
    {
        [SerializeField] private string actionName = null;
        [SerializeField] private ItemStackForSO primaryCost = ItemStackForSO.none;
        [SerializeField] private ItemStackForSO secondaryCost = ItemStackForSO.none;
        [SerializeField] private ItemStackForSO reward = ItemStackForSO.none;

        public string GetName() => actionName;
        public ItemStackForSO GetPrimaryCost() => primaryCost;
        public ItemStackForSO GetSecondaryCost() => secondaryCost;
        public ItemStackForSO GetReward() => reward;

        public TileAction(string actionName, ItemStackForSO primaryCost, ItemStackForSO secondaryCost, ItemStackForSO reward)
        {
            this.actionName = actionName;
            this.primaryCost = primaryCost;
            this.secondaryCost = secondaryCost;
            this.reward = reward;
        }
    }
}
