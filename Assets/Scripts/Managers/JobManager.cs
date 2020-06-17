using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private readonly HashSet<Worker> _employablePopulation = new HashSet<Worker>(); 
    private readonly HashSet<Worker> _workers = new HashSet<Worker>(); 
    private readonly HashSet<Worker> _retirees = new HashSet<Worker>();

    public void AddEmployableWorker(Worker worker) => _employablePopulation.Add(worker);

    public void EmployWorker(Worker worker)
    {
        _employablePopulation.Remove(worker);
        _workers.Add(worker);
    }


    public void RetireWorker(Worker worker)
    {
        _workers.Remove(worker);
        _retirees.Add(worker);
    }

    public void RemoveRetiree(Worker worker) => _retirees.Remove(worker);


    public int GetAmountOfEmployedWorkers() => _workers.Count;
    public int GetAmountOfRetirees() => _retirees.Count;
    public int GetAmountOfUnemployedWorkers() => _employablePopulation.Count;
}
