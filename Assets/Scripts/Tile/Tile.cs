using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileChange
    {
        Created,
        Destroyed,
    }

    public static event Action<Tile, TileChange> OnTileChanged;

    public event Action<string, ItemStack, ItemStack, ItemStack> OnConstructionFinished;

    private readonly bool isBuilding;
    private readonly int peopleCapacity;
    private readonly string actionTitle;
    private readonly ItemStack actionPrimaryCost;
    private readonly ItemStack actionSecondaryCost;
    private readonly ItemStack actionReward;

    private int daysToConstruct;

    public Tile(TileData data, int daysToConstruct)
    {
        isBuilding = data.IsBuilding();
        peopleCapacity = data.GetPeopleCapacity();
        actionTitle = data.GetActionName();
        actionPrimaryCost = data.GetActionPrimaryCost();
        actionSecondaryCost = data.GetActionSecondaryCost();
        actionReward = data.GetActionReward();

        this.daysToConstruct = daysToConstruct;

        OnTileChanged?.Invoke(this, TileChange.Created);
    }

    public int GetPeopleCapacity()
    {
        return peopleCapacity;
    }

    public bool CanExecuteTileAction()
    {
        return !SelectedBuildingUnconstructable() && CanCollectRewardAndPayCosts();
    }

    public bool SelectedBuildingUnconstructable()
    {
        return Building.selected != null && isBuilding;
    }

    public void ExecuteTileAction(TileBehaviour tile)
    {
        if (Building.selected != null)
        {
            if (!isBuilding) PlaceSelectedBuilding(tile);
        }
        else if(daysToConstruct > 0)
        {
            daysToConstruct--;
            PeopleManager.Instance.EmployPerson();
            if (daysToConstruct == 0) OnConstructionFinished?.Invoke(actionTitle, actionPrimaryCost, actionSecondaryCost, actionReward);
        }
        else if (CanCollectRewardAndPayCosts())
        {
            PayCosts();

            PeopleManager.Instance.EmployPerson();

            Inventory.Instance.Store(actionReward);
        }
    }

    public void DestroyBuilding(TileBehaviour tile)
    {
        if (Building.selected == null && isBuilding)
        {
            if (Building.selected != null) Building.selected.Deselect();

            PeopleManager.Instance.EmployPerson();

            tile.Construct(BoardManager.Instance.GetEmptyTile(), 0);
            OnTileChanged?.Invoke(this, TileChange.Destroyed);
        }
    }

    private bool CanCollectRewardAndPayCosts()
    {
        if (actionReward.IsNone()) return false;
        if (!Inventory.Instance.HasSpaceFor(actionReward)) return false;

        if (actionPrimaryCost.IsNone()) return true;
        if (!Inventory.Instance.Contains(actionPrimaryCost)) return false;

        if (actionSecondaryCost.IsNone()) return true;
        if (!Inventory.Instance.Contains(actionSecondaryCost)) return false;

        return true;
    }

    private void PlaceSelectedBuilding(TileBehaviour behaviour)
    {
        Building selected = Building.selected;
        selected.Deselect();

        Inventory.Instance.Retrieve(selected.data.GetCost());

        PeopleManager.Instance.EmployPerson();

        behaviour.Construct(selected.data.GetTile(), selected.data.GetDaysToConstruct());
    }

    private void PayCosts()
    {
        if (!actionPrimaryCost.IsNone())
        {
            Inventory.Instance.Retrieve(actionPrimaryCost);

            if (!actionSecondaryCost.IsNone()) Inventory.Instance.Retrieve(actionSecondaryCost);
        }
    }
}
