using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour, IInspectorSetter
{
    public static BoardManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TileBehaviour[] tiles = null;
    [SerializeField] private BuildingBehaviour[] buildings = null;

    private int startingMaxPeople;
    private TileData emptyTile;
    private TileData[] startingTiles;
    private BuildingData[] alreadyPlacedBuildings;
    private BuildingData[] allowedBuildings;

    private void Awake()
    {
        Instance = this;
    }

    public void Construct(TileData emptyTile, TileData[] startingTiles, BuildingData[] alreadyPlacedBuildings, BuildingData[] allowedBuildings)
    {
        this.emptyTile = emptyTile;

        this.startingTiles = startingTiles;
        this.alreadyPlacedBuildings = alreadyPlacedBuildings;
        this.allowedBuildings = allowedBuildings;

        AssignRandomBuildings();
        AssignRandomTiles();
    }

    public void SetInspectorComponents()
    {
        InspectorSetter.SetInspectorObjectsFoundByType(transform, ref tiles);
        InspectorSetter.SetInspectorObjectsFoundByType(transform, ref buildings);
    }

    public TileData GetEmptyTile()
    {
        return emptyTile;
    }

    public int GetStartingMaxPeople()
    {
        return startingMaxPeople;
    }

    public void SetBoardActive(bool active)
    {
        buildings[0].transform.parent.gameObject.SetActive(active);
        tiles[0].transform.parent.gameObject.SetActive(active);
    }

    public void AssignRandomBuildings()
    {
        List<BuildingData> buildingsToAssign = new List<BuildingData>(allowedBuildings);
        for (int i = 0; i < buildings.Length; i++)
        {
            int randomIndex = Random.Range(0, buildingsToAssign.Count);
            buildings[i].Construct(buildingsToAssign[randomIndex]);
            buildingsToAssign.RemoveAt(randomIndex);
        }
    }

    private void AssignRandomTiles()
    {
        startingMaxPeople = 0;
        List<TileData> tilesToAssign = new List<TileData>(startingTiles);
        foreach (BuildingData building in alreadyPlacedBuildings) tilesToAssign.Add(building.GetTile());

        for (int i = 0; i < tiles.Length; i++)
        {
            TileData dataSelected;
            if (tilesToAssign.Count == 0) dataSelected = startingTiles[Random.Range(0, startingTiles.Length)];
            else
            {
                int randomIndex = Random.Range(0, tilesToAssign.Count);
                dataSelected = tilesToAssign[randomIndex];
                tilesToAssign.RemoveAt(randomIndex);
            }
            tiles[i].Construct(dataSelected, 0);
            startingMaxPeople += dataSelected.GetPeopleCapacity();
        }
    }
}
