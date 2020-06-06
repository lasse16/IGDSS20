using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public BuildingPrefabs Buildings; 
    private List<Building> PlacedBuildings = new List<Building>();
    private BuildingType _currentActivePlacement = BuildingType.Lumberjack;


    public int GetUpkeepCost()
    {
        var total = 0;
        foreach (var building in PlacedBuildings)
        {
            total += building.GetUpkeepCost();
        }
        return total;
    }

    public bool PlaceBuildingOnTile(Building building, Tile tile)
    {
        var tileAllowed = building.GetSupportedTiles().Contains(tile.Type);
        if (tileAllowed)
        {
            building.tile = tile;
            building.gameObject.transform.position = tile.gameObject.transform.position;
            PlacedBuildings.Add(building);
        }

        return tileAllowed;
    }

    public Building GetBuildingOfType(BuildingType type)
    {
        GameObject prefab;

        //This can be much cleaner in C#8
        switch (type)
        {
            case BuildingType.Fishery:
                prefab = Buildings.Fishery;
                break;
            case BuildingType.Lumberjack:
                prefab = Buildings.LumberJack;
                break;
            case BuildingType.Sawmill:
                prefab = Buildings.Sawmill;
                break;
            case BuildingType.SheepFarm:
                prefab = Buildings.SheepFarm;
                break;
            case BuildingType.FrameworkKnitters:
                prefab = Buildings.FrameworkKnitters;
                break;
            case BuildingType.PotatoFarm:
                prefab = Buildings.PotatoFarm;
                break;
            case BuildingType.SchnappsDistillery:
                prefab = Buildings.SchnappsDistillery;
                break;
            default:
                throw new Exception($"Unknown building type - {Enum.GetName(typeof(BuildingType),type)}");
        }

        var obj =  Instantiate(prefab);
        return obj.GetComponent<Building>();
    }

    //TODO find better solution
    public void SetRequestedBuildingToLumberjack() => _currentActivePlacement = BuildingType.Lumberjack;
    public void SetRequestedBuildingToFishery() => _currentActivePlacement = BuildingType.Fishery;
    public void SetRequestedBuildingToSawmill() => _currentActivePlacement = BuildingType.Sawmill;
    public void SetRequestedBuildingToFrameworkKnitter() => _currentActivePlacement = BuildingType.FrameworkKnitters;
    public void SetRequestedBuildingToSheepfarm() => _currentActivePlacement = BuildingType.SheepFarm;
    public void SetRequestedBuildingToPotatoFarm() => _currentActivePlacement = BuildingType.PotatoFarm;
    public void SetRequestedBuildingToSchnappsDistillery() => _currentActivePlacement = BuildingType.SchnappsDistillery;
    public BuildingType GetCurrentPlacementBuilding() => _currentActivePlacement;

}