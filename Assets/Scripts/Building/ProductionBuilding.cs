using Assets.Scripts;
using IGDSS20.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IGDSS20.Buildings
{
    [Serializable]
    public class ProductionBuilding : EffiencyBuilding, IOccupiedBuilding
    {
        public ProductionBuildingStats ProductionBuildingStats;
        public List<Job> Jobs;

        private Dictionary<Worker, float> _timeSinceArrival;

        [SerializeField] private JobTracker _jobTracker;


        private float TimeInCurrentGenerationCycle;
        private bool productionRunning;


        void Start()
        {
            _effectiveGenerationInterval = CalculateEffectiveInterval(ProductionBuildingStats.ResourceGenerationInterval);
            _timeSinceArrival = new Dictionary<Worker, float>();
        }

        void Update()
        {

            if (!productionRunning)
            {
                TryStartProduction();
                return;
            }

            TimeInCurrentGenerationCycle += Time.deltaTime;

            if (TimeInCurrentGenerationCycle >= _effectiveGenerationInterval)
            {
                Storage.AddResource(ProductionBuildingStats.OutputResource, ProductionBuildingStats.OutputCount);
                TimeInCurrentGenerationCycle -= _effectiveGenerationInterval;

                _effectiveGenerationInterval = CalculateEffectiveInterval(ProductionBuildingStats.ResourceGenerationInterval);

                productionRunning = false;
            }

            SendHomeWorkersWithFinishedShift();
        }

        private void SendHomeWorkersWithFinishedShift()
        {
            foreach (var item in _timeSinceArrival)
            {
                var time = _timeSinceArrival[item.Key] += Time.deltaTime;
                if (time > ProductionBuildingStats.WorkingTime)
                {
                    SendHome(item.Key);
                }
            }
        }

        private void SendHome(Worker worker)
        {
            _timeSinceArrival.Remove(worker);
            worker.MoveHome(Tile);
        }

        private void TryStartProduction()
        {

            var inputResources = Storage.GetResourcesIfAvailable(ProductionBuildingStats.InputResources);
            bool allResourcesAvaiable = inputResources.Contains(false);

            productionRunning = allResourcesAvaiable;
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

            _efficiencyRequirements = GetComponents<IEfficiencyRequirement>().ToList();

            return tileAllowed;
        }

        public List<Worker> GetOccupants()
        {
            return Jobs.Select(x => x.Worker).Where(x => !(x is null)).ToList();
        }

        public void EmployWorker(Worker worker)
        {
            worker.ArrivedAtWork.AddListener(WorkerArrivedAtWork);
        }

        private void WorkerArrivedAtWork(Worker worker)
        {
            _timeSinceArrival.Add(worker, 0);
        }
    }
}