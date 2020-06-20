using IGDSS20.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IGDSS20.Jobs
{
    [CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/JobTracker")]
    public class JobTracker : ScriptableObject
    {
        public List<Job> Jobs;
        public bool JobsAvailable => Jobs.Count > 0;


        private void OnEnable()
        {
            //Ensure no jobs are left over from previous iteration
            Jobs = new List<Job>();
        }


        public void RegisterJobs(List<Job> jobs)
        {
            Jobs.AddRange(jobs);
        }

        public void RemoveJobs(ProductionBuilding building)
        {
            foreach (var job in Jobs)
            {
                if (job.Workplace == building)
                    Jobs.Remove(job);
            }
        }

        public Job GetRandomJob()
        {
            return Jobs.First();
        }

        public void Remove(Job job)
        {
            Jobs.Remove(job);
        }

        public void Add(Job job)
        {
            Jobs.Add(job);
        }
    }
}