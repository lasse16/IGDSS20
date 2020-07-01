using IGDSS20.Buildings;
using IGDSS20.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    // String für ugly to string method for potentialfield (just for testing)
    private string str;

    [Tooltip("This is the weight of the start tile. Assignment says 0.")]
    public int weightOfStartBuilding = 0;

    // Creates a list that contains a tile and its weight that depends on its type & the path to it. 
    public List<(Tile,int)> createPotentialFieldMapFor(Building building)
    {
        // add start tile and its weight
        Tile buildingsTile = building.Tile;
        List <(Tile, int)> potentialFields = new List<(Tile, int)>();
        potentialFields.Add((buildingsTile, weightOfStartBuilding));

        // start recursion
        addNeighboringPotentialFields(buildingsTile, weightOfStartBuilding, potentialFields);

        // return result
        return potentialFields;
    }

    // this is recursive
    // fills the list of potential fields with tiles and their weights. 
    private void addNeighboringPotentialFields(Tile lastTile, int lastTotalWeight, List<(Tile, int)> potentialfields)
    {
        foreach (Tile tile in lastTile.NeighbouringTiles)
        {
            // weight of tile =  weight of path + weight from time
            int totalWeight = lastTotalWeight + getWeightForTile(tile);
            
            // check for duplicate tiles
            // choose lowest value
            if (!potentialfields.Exists(item => item.Item1 == tile))
            {
                // add complete value of path
                potentialfields.Add((tile, totalWeight));
                addNeighboringPotentialFields(tile, totalWeight, potentialfields);
            }
            else
            {
                // a value for a path to this tile has been already stored
                (Tile, int) duplicate = potentialfields.Find(item => item.Item1 == tile);
                if(duplicate.Item2 > totalWeight)
                {
                    // new path to this tile is better than saved one --> replace old value
                    potentialfields.Remove(duplicate);
                    potentialfields.Add((tile, totalWeight));

                }
                //else
                //{
                //    // nothing to do because the new path is not better than the known one
                //}
            }
        }

    }


    /*
     * ugly to string method to check saved values in potenfialFields
     * 
     * CAN BE DELETED AFTER TESTING
     */ 
    private string potentialFieldsToString(List<(Tile, int)> potFileds)
    {
        str = "";

        foreach ((Tile, int) item in potFileds){
            str += $"({item.Item1}, {item.Item2}), \n";
        }

        return str;
    }


    /*
     * Return weight for tile type. 
     * 
     * possible: return also bool that saves, if there has benn an error (default: ...)
     * --> catch in Potential Field Map Generation --> wight of current field is int.Infinite
     */
    private int getWeightForTile(Tile tile) { 
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
