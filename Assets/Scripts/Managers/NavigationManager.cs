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
        Tile buildingsTile = building.Tile;
        var potentialFields = new PotentialMap(buildingsTile, _tileWeights);

        return potentialFields;
    }
}
