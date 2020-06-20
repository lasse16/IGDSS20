using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/PopulationSet")]
public class PopulationSet : ScriptableObject
{
    public WorkerEvent WorkerBirth;
    public WorkerEvent WorkerComingOfAge;
    public WorkerEvent WorkerRetiring;
    public WorkerEvent WorkerDeath;


    private readonly HashSet<Worker> _children = new HashSet<Worker>();
    private readonly HashSet<Worker> _workers = new HashSet<Worker>();
    private readonly HashSet<Worker> _retirees = new HashSet<Worker>();

    public void AddChild(Worker worker) => _children.Add(worker);
    public void AddWorker(Worker worker) => _workers.Add(worker);
    public void AddRetiree(Worker worker) => _retirees.Add(worker);


    public void RemoveChild(Worker worker) => _children.Remove(worker);
    public void RemoveWorker(Worker worker) => _workers.Remove(worker);
    public void RemoveRetiree(Worker worker) => _retirees.Remove(worker);

    public List<Worker> GetWorkers() => _workers.ToList();
    public List<Worker> GetRetirees() => _retirees.ToList();
    public List<Worker> GetChildren() => _children.ToList();

    /// <summary>
    /// Remove a worker of unknown stand
    /// </summary>
    /// <param name="worker"></param>
    public void RemoveFromPopulation(Worker worker)
    {
        if (_retirees.Contains(worker))
        {
            _retirees.Remove(worker);
        }
        else if (_children.Contains(worker))
        {
            _children.Remove(worker);
        }
        else if (_workers.Contains(worker))
        {
            _workers.Remove(worker);
        }
    }

    //These are here, so that they can be subscribed to, based on a population instead of having to subscribe to each individual worker
    public void PropagateWorkerBirth(Worker worker) => WorkerBirth.Invoke(worker);
    public void PropagateWorkerComingOfAge(Worker worker) => WorkerComingOfAge.Invoke(worker);
    public void PropagateWorkerRetiring(Worker worker) => WorkerRetiring.Invoke(worker);
    public void PropagateWorkerDeath(Worker worker) => WorkerDeath.Invoke(worker);
}
