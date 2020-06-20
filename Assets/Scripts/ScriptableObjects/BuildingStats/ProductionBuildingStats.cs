using IGDSS20.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/ProductionBuildingStats")]
public class ProductionBuildingStats : GeneralBuildingStats
{
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

    [Tooltip("Number of workers required for full efficiency")]
    public int JobsAvailable;
}

