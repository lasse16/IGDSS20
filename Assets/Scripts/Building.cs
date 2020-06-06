using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GeneralBuildingStats GeneralBuildingStats;
    public WareHouse wareHouse;


    [HideInInspector]
    public Tile tile;

    private float TimeInCurrentGenerationCycle;
    private float EffectiveGenerationTime;
    private bool productionRunning;

    void Start()
    {
        var efficiency = CalculateEfficiency();
        EffectiveGenerationTime = (1 / efficiency) * GeneralBuildingStats.ResourceGenerationInterval;
    }

    void Update()
    {

        if (!productionRunning)
        {
            TryStartProduction();
            return;
        }

        TimeInCurrentGenerationCycle += Time.deltaTime;

        if (TimeInCurrentGenerationCycle >= EffectiveGenerationTime)
        {
            wareHouse.AddResource(GeneralBuildingStats.OutputResource, GeneralBuildingStats.OutputCount);
            TimeInCurrentGenerationCycle -= EffectiveGenerationTime;
            productionRunning = false;
        }
    }


    private void TryStartProduction()
    {

        var inputResources = wareHouse.GetResourcesIfAvailable(GeneralBuildingStats.InputResources);
        bool allResourcesAvaiable = inputResources.Contains(false);

        productionRunning = allResourcesAvaiable;
    }


    private float CalculateEfficiency()
    {
        if (GeneralBuildingStats.EfficientNeighboringTile == TileType.None)
            return 1;

        int counter = 0;
        foreach (var item in tile.NeighbouringTiles)
        {
            if (item.Type == GeneralBuildingStats.EfficientNeighboringTile)
                counter++;
        }

        var efficiency = (counter - GeneralBuildingStats.MinEfficientNeigbor) / (GeneralBuildingStats.MaxEfficientNeighbor - GeneralBuildingStats.MinEfficientNeigbor);
        return Mathf.Clamp(efficiency, 0, 1);
    }


    public int GetUpkeepCost()
    {
        return GeneralBuildingStats.UpkeepCost;
    }

    public List<TileType> GetSupportedTiles()
    {
        return GeneralBuildingStats.AllowedTileTypes;
    }
}
