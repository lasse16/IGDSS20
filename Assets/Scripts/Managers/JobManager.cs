using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    [SerializeField] private PopulationSet _population;

    private readonly Dictionary<ProductionBuilding, int> _freeJobs = new Dictionary<ProductionBuilding, int>();
    private readonly Dictionary<Worker, ProductionBuilding> _workplaces = new Dictionary<Worker, ProductionBuilding>();
    [SerializeField] [ReadOnly] private List<Worker> _unemployedWorkers = new List<Worker>();

    private void Awake()
    {
        _population.WorkerComingOfAge.AddListener(TrackWorker);
        _population.WorkerRetiring.AddListener(FreeOccupiedJob);
        _population.WorkerDeath.AddListener(FreeOccupiedJob);
    }

    private void Update()
    {
        if (_freeJobs.Count > 0 && GetAmountOfUnemployedWorkers() > 0)
        {
            foreach (var worker in _unemployedWorkers)
                    TryEmployWorker(worker);
        }
    }

    public int AmountOfRetirees => _population.GetRetirees().Count;
    public int GetAmountOfEmployedWorkers() => _workplaces.Count;
    public int GetAmountOfUnemployedWorkers() => _unemployedWorkers.Count;

    public void TrackWorker(Worker worker)
    {
        _unemployedWorkers.Add(worker);
        if (TryEmployWorker(worker))
        {
            _unemployedWorkers.Remove(worker);
        }
    }

    public bool TryEmployWorker(Worker worker)
    {
        if (!(_freeJobs.Count > 0))
            return false;

        //TODO OPTIONAL Priority system
        //Implement priority system for work place finding
        var job = _freeJobs.First();
        var building = job.Key;
        var quantity = job.Value;
        quantity -= 1;

        _workplaces.Add(worker, building);
        worker.Employ(building);

        if (quantity == 0)
            _freeJobs.Remove(building);
        else
            _freeJobs[building] = quantity;

        return true;
    }

    public void RegisterJobs(ProductionBuilding building, int quantity)
    {
        if (!_freeJobs.ContainsKey(building))
        {
            _freeJobs.Add(building, default);
        }
        _freeJobs[building] = quantity;
    }

    public void RemoveJobs(ProductionBuilding building)
    {
        if (_freeJobs.ContainsKey(building))
            _freeJobs.Remove(building);

        foreach (var worker in _workplaces)
        {
            if (worker.Value == building)
            {
                _workplaces.Remove(worker.Key);
                _unemployedWorkers.Add(worker.Key);
            }
        }
    }

    private void FreeJob(Worker worker)
    {
        var job = _workplaces[worker];
        _workplaces.Remove(worker);

        if (!_freeJobs.ContainsKey(job))
            _freeJobs.Add(job, 0);

        _freeJobs[job] += 1;
    }

    private bool HasJob(Worker worker) => _workplaces.ContainsKey(worker);

    private void FreeOccupiedJob(Worker worker)
    {
        if (HasJob(worker))
        {
            FreeJob(worker);
        }
    }

}
