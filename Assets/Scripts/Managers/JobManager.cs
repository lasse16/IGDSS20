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

    private void Awake()
    {
        _population.WorkerDeath.AddListener(FreeOccupiedJob);
    }

    private void Update()
    {
        if (_freeJobs.Count > 0 && GetAmountOfUnemployedWorkers() > 0)
        {
            //TODO OPTIONAL Optimize amount of calls 
            var workers = _population.GetWorkers();
            foreach (var worker in workers)
            {
                if (!HasJob(worker))
                {
                    EmployWorker(worker);
                }
            }
        }
    }

    internal int AmountOfRetirees => _population.GetRetirees().Count;

    internal int GetAmountOfEmployedWorkers() => _workplaces.Count;

    internal int GetAmountOfUnemployedWorkers()
    {
        return _population.GetWorkers().Count - GetAmountOfEmployedWorkers();
    }
    public void EmployWorker(Worker worker)
    {
        if (!(_freeJobs.Count > 0))
            return;

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
                _workplaces.Remove(worker.Key);
        }
    }
}
