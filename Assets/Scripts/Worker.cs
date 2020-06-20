﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Worker : MonoBehaviour
    {
        [SerializeField] private WorkerEvent Birth;
        [SerializeField] private WorkerEvent ComingOfAge;
        [SerializeField] private WorkerEvent Retiring;
        [SerializeField] private WorkerEvent Death;

        public HousingBuilding Home;
        public ProductionBuilding Workplace;

        private List<IHappinessRequirement> _happinessRequirements;



        [Range(0, float.MaxValue)]
        [SerializeField] private int _age;
        [Range(0, float.MaxValue)]
        [SerializeField] private float _happiness;

        private const float ageIntervalInSeconds = 15;
        private float timeSinceLastAgeIncrease;

        private void OnEnable()
        {
            _happinessRequirements = GetComponents<IHappinessRequirement>().ToList();
        }

        public void Setup(HousingBuilding home)
        {
            Home = home;

            _age = 0;
            _happiness = 0;
            timeSinceLastAgeIncrease = 0;

            Birth.Invoke(this);
        }

        private void Update()
        {
            timeSinceLastAgeIncrease += Time.deltaTime;

            //TODO Extract time based method call
            //Maybe check out InvokeRepeating()
            if (timeSinceLastAgeIncrease > ageIntervalInSeconds)
            {
                _age++;
                timeSinceLastAgeIncrease -= ageIntervalInSeconds;
                CheckAge(_age);
            }

            _happiness = CalculateHappiness();
        }

        private float CalculateHappiness()
        {
            var total = 0f;
            var totalImportance = 0;

            foreach (var requirement in _happinessRequirements)
            {
                var fulfillment = requirement.CheckFulfillment();
                var importance = requirement.GetImportance();

                totalImportance += importance;

                total += fulfillment * importance;
            }

            if (totalImportance == 0)
                return 1;

            return total / totalImportance;
        }

        internal void Employ(ProductionBuilding building)
        {
            Workplace = building;
        }

        private void CheckAge(int age)
        {
            switch (age)
            {
                case var _ when age == 14:
                    ComingOfAge.Invoke(this);
                    break;
                case var _ when age == 64:
                    Retiring.Invoke(this);
                    break;
                case var _ when age == 100:
                    Death.Invoke(this);
                    break;
                default:
                    break;
            }
        }

        public float GetHappiness() => _happiness;

        public void Kill(Worker worker)
        {
            Destroy(gameObject);
        }

        [ContextMenu("GrowUp")]
        public void AgeIncrease() => _age = 14;

    }
}
