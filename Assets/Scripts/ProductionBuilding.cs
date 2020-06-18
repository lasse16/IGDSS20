using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building
{
    public  ProductionBuildingStats ProductionBuildingStats;

    private float TimeInCurrentGenerationCycle;
    private float EffectiveGenerationTime;
    private bool productionRunning;


    #region Manager References
    JobManager _jobManager; //Reference to the JobManager
    #endregion

    #region Jobs
    public List<Job> _jobs; // List of all available Jobs. Is populated in Start()
    #endregion

    void Start()
    {
        // register job vacancies at JobManager
        RegisterJobs();

        // TODO
        // efficiency have to contain/ be affected by happiness and count of workers. 
        var efficiency = CalculateEfficiency();
        EffectiveGenerationTime = (1 / efficiency) * ProductionBuildingStats.ResourceGenerationInterval;
    }

    void Update()
    {
        // TODO: update happiness and worker count efficiency. Former could be part of building

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



    public int GetUpkeepCost()
    {
        return ProductionBuildingStats.UpkeepCost;
    }

    public List<TileType> GetSupportedTiles()
    {
        return ProductionBuildingStats.AllowedTileTypes;
    }


    /*
     * register available jobs at JobManager
     */ 
    public void registerJobs()
    {
        for(int i = 1; 1 > ProductionBuildingStats.JobsAvailable; i++)
        {
            _jobs.Add(new Job(this));
        };

        _jobManager.registerAvailableJobs(_jobs);
    }
}
