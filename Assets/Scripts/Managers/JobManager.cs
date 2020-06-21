using Assets.Scripts;
using IGDSS20.Buildings;
using IGDSS20.Helpers;
using IGDSS20.Jobs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class JobManager : MonoBehaviour
{
    [SerializeField] private PopulationSet _population;
    [SerializeField] private JobTracker _freeJobs;

    [SerializeField] private List<Job> _workplaces = new List<Job>();
    [SerializeField] private List<Worker> _unemployedWorkers = new List<Worker>();

    private void Awake()
    {
        _population.WorkerComingOfAge.AddListener(TrackWorker);
        _population.WorkerRetiring.AddListener(RetireWorker);
        _population.WorkerDeath.AddListener(FreeOccupiedJob);
    }

    private void Update()
    {
        if (_freeJobs.JobsAvailable && GetAmountOfUnemployedWorkers() > 0)
        {
            foreach (var worker in _unemployedWorkers)
                TryEmployWorker(worker);
        }
    }

    public int GetAmountOfRetirees() => _population.GetRetirees().Count;
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
        if (!_freeJobs.JobsAvailable)
            return false;

        //TODO OPTIONAL Priority system
        //Implement priority system for work place finding
        var job = _freeJobs.GetRandomJob();
        _freeJobs.Remove(job);

        _workplaces.Add(job);

        worker.Employ(job);
        job.Employ(worker);


        return true;
    }


    public void RemoveJobs(ProductionBuilding building)
    {
        _freeJobs.RemoveJobs(building);

        foreach (var worker in _workplaces)
        {
            if (worker.Workplace == building)
            {
                _workplaces.Remove(worker);
                _unemployedWorkers.Add(worker.Worker);
            }
        }
    }

    private void FreeJob(Worker worker)
    {
        var job = _workplaces.Find(x => x.Worker == worker);
        _workplaces.Remove(job);
        _freeJobs.Add(job);

        job.Fire();
        worker.Fire();
    }

    private bool HasJob(Worker worker)
    {
        foreach (var item in _workplaces)
        {
            if (worker == item.Worker)
                return true;
        }
        return false;
    }

    private void FreeOccupiedJob(Worker worker)
    {
        if (HasJob(worker))
        {
            FreeJob(worker);
        }
    }

    private void RetireWorker(Worker worker)
    {
        if (HasJob(worker))
        {
            FreeJob(worker);
        }
        else
            _unemployedWorkers.Remove(worker);
    }

    [ContextMenu("PrintWorkers")]
    private void PrintWorkers()
    {
        foreach (var item in _workplaces)
        {
            print(item);
        }
    }
}


