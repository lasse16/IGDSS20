using IGDSS20.Assets.Scripts.Navigation;
using IGDSS20.Buildings;
using IGDSS20.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] private TileWeights _tileWeights;

    /// <summary>
    /// Creates a list that contains a tile and its weight that depends on its type & the path to it. 
    /// </summary>
    /// <param name="building">Building to use as starting point for the potential map</param>
    public PotentialMap CreatePotentialFieldMapFor(Building building)
    {
        // add start tile and its weight
        Tile buildingsTile = building.Tile;
        var potentialFields = new PotentialMap(buildingsTile);

        // start recursion
        AddNeighboringPotentialFields(buildingsTile, 0, potentialFields);

        // return result
        return potentialFields;
    }

    // this is recursive
    // fills the list of potential fields with tiles and their weights. 
    private void AddNeighboringPotentialFields(Tile lastTile, int lastTotalWeight, PotentialMap potentialfields)
    {
        foreach (Tile tile in lastTile.NeighbouringTiles)
        {
            int totalWeight = lastTotalWeight + _tileWeights.GetWeightForTileType(tile.Type);

            if (potentialfields.Add(tile, totalWeight))
            {
                AddNeighboringPotentialFields(tile, totalWeight, potentialfields);
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
}
