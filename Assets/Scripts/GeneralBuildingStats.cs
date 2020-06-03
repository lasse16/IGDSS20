using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="data",menuName ="ScribtableObjects/GeneralBuildingStats")]
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

    [Tooltip("Minimum and maximum number of surrounding tiles its efficiency scales with")]
    [Range(0, 6)]
    //TODO Ensure min is alwaays less than max
    public int MinEfficientNeigbor, MaxEfficientNeighbor;
    [Tooltip("Type of preferred neighbouring tile")]
    public TileType EfficientNeighboringTile;

    //TODO Limit input resources in range 0-2
    [Tooltip("Input resources required to start production")]
    public List<ResourceType> InputResources;
    [Tooltip("Resource this building outputs each cycle")]
    public ResourceType OutputResource;
    [Tooltip("Count of resource outputted each cycle")]
    public int OutputCount;
    [Tooltip("Duration of a production cycle in seconds")]
    public float ResourceGenerationInterval;


}
