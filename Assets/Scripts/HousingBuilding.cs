using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HousingBuilding : Building
{
    public HousingBuildingStats HousingBuildingStats;
    //TODO Make better -> pool
    public GameObject WorkerPrefab;
    private List<Worker> _inhabitants = new List<Worker>();

    private float _efficency;
    private float _timeInGenerationInterval;
    private float _effectiveGenerationinterval;

    private void Start()
    {


        _efficency = CalculateEfficiency();
        _effectiveGenerationinterval = CalculateEffectiveInterval();
    }

    private void AddWorker()
    {
        if (_inhabitants.Count >= HousingBuildingStats.MaxInhabitants)
            return;

        var worker = Instantiate(WorkerPrefab);
        var script = worker.GetComponent<Worker>();
        script.Setup(this);
        _inhabitants.Add(script);
    }

    private float CalculateEffectiveInterval()
    {
        return (1 / _efficency) * HousingBuildingStats.WorkerGenerationIntervalInSeconds;
    }

    private void Update()
    {
        _timeInGenerationInterval += Time.deltaTime;
        if (_timeInGenerationInterval > _effectiveGenerationinterval)
        {
            _timeInGenerationInterval -= _effectiveGenerationinterval;

            AddWorker();

            _efficency = CalculateEfficiency();
            _effectiveGenerationinterval = CalculateEffectiveInterval();
        }
    }




    private float CalculateEfficiency()
    {
        var totalHappiness = 0f;
        foreach (var worker in _inhabitants)
        {
            totalHappiness += worker.GetHappiness();
        }
        return totalHappiness / _inhabitants.Count;
    }

    public void RemoveWorker(Worker worker)
    {
        //Use exception to avoid checking 'contains' for every worker
        try
        {
            _inhabitants.Remove(worker);
        }
        catch (ArgumentException)
        {
            return;
        }
    }

    public override bool ConstructOnTile(Tile tile, IStorage storage)
    {
        if (!GeneralBuildingStats.AllowedTileTypes.Contains(tile.Type))
            return false;

        for (int i = 0; i < HousingBuildingStats.StartingInhabitants; i++)
            AddWorker();

        Tile = tile;
        Storage = storage;
        gameObject.transform.position = tile.gameObject.transform.position;

        return true;
    }
}
