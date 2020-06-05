using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public BuildingPrefabs Buildings; 
    private RunTimeSet<Building> PlacedBuildings;


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
            PlacedBuildings.Add(building);
        }

        return tileAllowed;
    }

    public GameObject GetBuildingOfType(BuildingType type)
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
                throw new Exception("Unknown building type");
        }

        return Instantiate(prefab);
    }
}