using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{

    private List<Job> _availableJobs = new List<Job>();
    public List<Worker> _unoccupiedWorkers = new List<Worker>();
    public List<Job> _occupiedJobs = new List<Job > ();



    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleUnoccupiedWorkers();
    }
    #endregion


    #region Methods

    private void HandleUnoccupiedWorkers()
    {
        if (_unoccupiedWorkers.Count > 0 && _availableJobs.Count > 0)
        {
            // assign job and remove worker from onoccupied workers list
            foreach(Worker w in _unoccupiedWorkers){
                assignRandomJob(w);
                RemoveWorker(w);
            };
           



        }
    }

    /*
     * Save new available jobs in list. 
     */ 
    public void registerAvailableJobs(List<Job> jobs)
    {
        foreach(Job job in jobs)
        {
            _availableJobs.Add(job);
        }
    }

    public void RegisterWorker(Worker w)
    {
        _unoccupiedWorkers.Add(w);
    }



    public void RemoveWorker(Worker w)
    {
        _unoccupiedWorkers.Remove(w);
    }

    #endregion
    /* 
     * Handles death of worker by making his job available again if he had one
     * or deleting him from the list of unemployed workers. 
     */ 
    public void handleDeathOf(Worker w)
    {
        if (_unoccupiedWorkers.Contains(w))
        {
            // deceased worker had no job 
            RemoveWorker(w);
        }
        else if (_occupiedJobs.Exists(job => job._worker == w))
        {
            Job vacancy = _occupiedJobs.Find(job => job._worker == w);
            // create new available Job
            vacancy.RemoveWorker(w);
            _availableJobs.Add(vacancy);
            
            // Remove assignment of deceased worker
            _occupiedJobs.Remove(vacancy);        }

    }

    private void assignRandomJob(Worker w)
    {
        // get random job
        int rand = Random.Range(0, _availableJobs.Count);
        Job job = _availableJobs[rand];

        // assign worker to job
       job.AssignWorker(w);
        _occupiedJobs.Add(job);

        // remove job from available jobs list
        _availableJobs.RemoveAt(rand);

    }
}
