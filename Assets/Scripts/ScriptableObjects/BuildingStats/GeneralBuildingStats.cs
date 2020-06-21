using System.Collections.Generic;
using UnityEngine;
using System;
using IGDSS20.Enums;

[CreateAssetMenu(fileName ="data",menuName ="ScriptableObjects/GeneralBuildingStats")]
public class GeneralBuildingStats : ScriptableObject
{
    [Tooltip("Type of building")]
    public BuildingType BuildingType;
    [Tooltip("Type of tiles this building can be built on")]
    public List<TileType> AllowedTileTypes;


    [Tooltip("Money cost per economy cycle")]
    public int UpkeepCost;
    [Tooltip("Money required to build this building")]
    public int BuildCostMoney;
    [Tooltip("Planks required to build this building")]
    public int BuildCostPlanks;
}
