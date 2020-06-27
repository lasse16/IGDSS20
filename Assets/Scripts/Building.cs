using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GeneralBuildingStats GeneralBuildingStats;
    public IStorage WareHouse;

    [HideInInspector]
    public Tile tile;

    // for navigation
    public List<(Tile, int)> potentialFieldsList;
}

