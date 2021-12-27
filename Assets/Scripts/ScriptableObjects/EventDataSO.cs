using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventDataSO : ScriptableObject
{
    [SerializeField] private string eventName = null;
    [SerializeField] private int daysToPrepare = 0;
    [SerializeField] private ItemStackForSO cost = ItemStackForSO.none;
    [SerializeField] private ItemStackForSO reward = ItemStackForSO.none;

    public string GetName() => eventName;
    public int GetDaysToPrepare() => daysToPrepare;
    public ItemStackForSO GetCost() => cost;
    public ItemStackForSO GetReward() => reward;
}
