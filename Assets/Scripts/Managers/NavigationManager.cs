using IGDSS20.Assets.Scripts.Navigation;
using IGDSS20.Buildings;
using IGDSS20.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    /// <summary>
    /// Creates a list that contains a tile and its weight that depends on its type & the path to it. 
    /// </summary>
    /// <param name="building">Building to use as starting point for the potential map</param>
    public PotentialMap createPotentialFieldMapFor(Building building)
    {
        // add start tile and its weight
        Tile buildingsTile = building.Tile;
        var potentialFields = new PotentialMap(buildingsTile);

        // start recursion
        addNeighboringPotentialFields(buildingsTile, 0, potentialFields);

        // return result
        return potentialFields;
    }

    // this is recursive
    // fills the list of potential fields with tiles and their weights. 
    private void addNeighboringPotentialFields(Tile lastTile, int lastTotalWeight, PotentialMap potentialfields)
    {
        foreach (Tile tile in lastTile.NeighbouringTiles)
        {
            int totalWeight = lastTotalWeight + getWeightForTile(tile);

            if (potentialfields.Add(tile, totalWeight))
            {
                addNeighboringPotentialFields(tile, totalWeight, potentialfields);
            }
            else
            {
                var previousWeight = potentialfields.GetWeight(tile);
                if (totalWeight < previousWeight)
                {
                    potentialfields.UpdateWeight(tile, totalWeight);
                }
            }
        }

    }

    /*
     * Return weight for tile type. 
     * 
     * possible: return also bool that saves, if there has benn an error (default: ...)
     * --> catch in Potential Field Map Generation --> wight of current field is int.Infinite
     */
    private int getWeightForTile(Tile tile)
    {
        switch (tile.Type)
        {
            case TileType.Water:
                return 30;
            case TileType.Sand:
                return 2;
            case TileType.Grass:
                return 1;
            case TileType.Forest:
                return 2;
            case TileType.Stone:
                return 1;
            case TileType.Mountain:
                return 3;
            default:
                return int.MaxValue;
        }
    }
}
