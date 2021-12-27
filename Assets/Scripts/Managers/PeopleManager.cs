using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public static PeopleManager Instance { get; private set; }

    private bool constructedBefore;

    private bool shouldBeMax;
    private int unemployedPeople;
    private int maxPeople;

    private void Awake()
    {
        Instance = this;
        constructedBefore = false;
    }

    public void Construct(int maxPeople)
    {
        if(!constructedBefore)
        {
            Tile.OnTileChanged += UpdateMaxPeople;
            DayManager.OnDayEnded += ResetUnemployedPeople;
            constructedBefore = true;
        }

        this.maxPeople = maxPeople;
        UIManager.Instance.UpdateMaxPeopleMesh(maxPeople);

        ResetUnemployedPeople();
    }

    public void ResetUnemployedPeople()
    {
        shouldBeMax = true;
        unemployedPeople = maxPeople;
        UIManager.Instance.UpdateUnemployedPeopleMesh(unemployedPeople);
    }

    public void EmployPerson()
    {
        shouldBeMax = false;
        unemployedPeople--;
        UIManager.Instance.UpdateUnemployedPeopleMesh(unemployedPeople);

        if (unemployedPeople == 0) DayManager.Instance.PassDay();
    }

    private void UpdateMaxPeople(Tile tile, Tile.TileChange change)
    {
        if (tile.GetPeopleCapacity() == 0) return;

        switch (change)
        {
            case Tile.TileChange.Created:
                maxPeople += tile.GetPeopleCapacity();
                break;
            case Tile.TileChange.Destroyed:
                maxPeople -= tile.GetPeopleCapacity();
                break;
            default:
                break;
        }
        UIManager.Instance.UpdateMaxPeopleMesh(maxPeople);

        if (shouldBeMax) ResetUnemployedPeople();

        if (maxPeople == 0)
        {
            UIManager.Instance.ChangePeopleMeshesColorToFail();

            MissionManager.Instance.MissionFailed();
        }
    }
}
