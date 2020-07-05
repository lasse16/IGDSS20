using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IGDSS20.Buildings
{
    public class HousingBuilding : EffiencyBuilding, IOccupiedBuilding
    {
        public HousingBuildingStats HousingBuildingStats;
        //TODO Make better -> pool
        public GameObject WorkerPrefab;
        private List<Worker> _inhabitants = new List<Worker>();

        private float _timeInGenerationInterval;

        private void Start()
        {
            _effectiveGenerationInterval = CalculateEffectiveInterval(HousingBuildingStats.WorkerGenerationIntervalInSeconds);
        }

        private void AddWorker()
        {
            if (_inhabitants.Count >= HousingBuildingStats.MaxInhabitants)
            {
                return;
            }

            var worker = Instantiate(WorkerPrefab);
            worker.transform.SetParent(transform);

            var script = worker.GetComponent<Worker>();
            script.Setup(this);
            _inhabitants.Add(script);
        }

        private void Update()
        {
            _timeInGenerationInterval += Time.deltaTime;
            if (_timeInGenerationInterval > _effectiveGenerationInterval)
            {
                _timeInGenerationInterval -= _effectiveGenerationInterval;

                AddWorker();

                _effectiveGenerationInterval = CalculateEffectiveInterval(HousingBuildingStats.WorkerGenerationIntervalInSeconds);
            }
        }

        public void RemoveWorker(Worker worker)
        {
            _inhabitants.Remove(worker);
        }

        public override bool ConstructOnTile(Tile tile, IStorage storage)
        {
            if (!GeneralBuildingStats.AllowedTileTypes.Contains(tile.Type))
            {
                return false;
            }

            for (int i = 0; i < HousingBuildingStats.StartingInhabitants; i++)
            {
                AddWorker();
            }

            Tile = tile;
            Storage = storage;
            gameObject.transform.position = tile.gameObject.transform.position;
            _efficiencyRequirements = GetComponents<IEfficiencyRequirement>().ToList();

            return true;
        }

        public List<Worker> GetOccupants() => _inhabitants;
    }
}