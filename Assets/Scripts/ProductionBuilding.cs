using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building
{
    public ProductionBuildingStats ProductionBuildingStats;
    public List<Job> Jobs;

    [SerializeField] private JobTracker _jobTracker;

    private float TimeInCurrentGenerationCycle;
    private float EffectiveGenerationTime;
    private bool productionRunning;


    void Start()
    {
        var efficiency = CalculateEfficiency();
        EffectiveGenerationTime = (1 / efficiency) * ProductionBuildingStats.ResourceGenerationInterval;
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
            WareHouse.AddResource(ProductionBuildingStats.OutputResource, ProductionBuildingStats.OutputCount);
            TimeInCurrentGenerationCycle -= EffectiveGenerationTime;
            productionRunning = false;
        }
    }


    private void TryStartProduction()
    {

        var inputResources = WareHouse.GetResourcesIfAvailable(ProductionBuildingStats.InputResources);
        bool allResourcesAvaiable = inputResources.Contains(false);

        productionRunning = allResourcesAvaiable;
    }


    private float CalculateEfficiency()
    {
        if (ProductionBuildingStats.EfficientNeighboringTile == TileType.None)
            return 1;

        int counter = 0;
        foreach (var item in tile.NeighbouringTiles)
        {
            if (item.Type == ProductionBuildingStats.EfficientNeighboringTile)
                counter++;
        }

        var efficiency = (counter - ProductionBuildingStats.MinEfficientNeigbor) / (ProductionBuildingStats.MaxEfficientNeighbor - ProductionBuildingStats.MinEfficientNeigbor);
        return Mathf.Clamp(efficiency, 0, 1);
    }

    public override bool ConstructOnTile(Tile tile, IStorage storage)
    {
        var tileAllowed = GeneralBuildingStats.AllowedTileTypes.Contains(tile.Type);
        if (tileAllowed)
        {
            Tile = tile;
            Storage = storage;
            gameObject.transform.position = tile.gameObject.transform.position;

            var jobs = new List<Job>();
            for (int i = 0; i < ProductionBuildingStats.JobsAvailable; i++)
            {
                jobs.Add(new Job(this));
            }

            Jobs = jobs;
            _jobTracker.RegisterJobs(Jobs);
        }


        return tileAllowed;
    }
}
