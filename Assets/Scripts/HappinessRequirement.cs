using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(IOccupiedBuilding))]
    public class HappinessRequirement : MonoBehaviour, IEfficiencyRequirement
    {
        [SerializeField] private int _importance;
        private IOccupiedBuilding _building;

        public void Awake()
        {
            _building = GetComponent<IOccupiedBuilding>();
        }

        public float CheckFulfillment()
        {
            var totalHappiness = 0f;
            List<Worker> occupants = _building.GetOccupants();
            foreach (var worker in occupants)
            {
                totalHappiness += worker.GetHappiness();
            }
            return totalHappiness / occupants.Count;

        }

        public int GetImportance() => _importance;
    }
}
