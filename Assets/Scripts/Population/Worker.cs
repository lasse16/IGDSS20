﻿using IGDSS20.Buildings;
using IGDSS20.Helpers;
using IGDSS20.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Worker : MonoBehaviour
    {
        public HousingBuilding Home;
        public ProductionBuilding Workplace;

        [SerializeField][ReadOnly] private int _age;
        [SerializeField][ReadOnly] private float _happiness;


        [SerializeField] private WorkerEvent Birth;
        [SerializeField] private WorkerEvent ComingOfAge;
        [SerializeField] private WorkerEvent Retiring;
        [SerializeField] private WorkerEvent Death;




        private List<IHappinessRequirement> _happinessRequirements;

        private const float ageIntervalInSeconds = 1;
        private float timeSinceLastAgeIncrease;

        public float GetHappiness() => _happiness;

        public void Kill(Worker worker)
        {
            Destroy(gameObject);
        }

        public void Employ(Job job) => Workplace = job.Workplace;


        public void Setup(HousingBuilding home)
        {
            Home = home;

            _age = 0;
            _happiness = 0;
            timeSinceLastAgeIncrease = 0;
            _happinessRequirements = GetComponents<IHappinessRequirement>().ToList();

            Birth.Invoke(this);
        }

        private void Update()
        {
            timeSinceLastAgeIncrease += Time.deltaTime;

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

        public void Fire()
        {
            Workplace = null;   
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
        [ContextMenu("Invoke/Birth")]
        public void ComingOfBirthInvoke() => Birth.Invoke(this);
        [ContextMenu("Invoke/ComingOfAge")]
        public void ComingOfAgeInvoke() => ComingOfAge.Invoke(this);
        [ContextMenu("Invoke/Retiring")]
        public void RetiringInvoke() => Retiring.Invoke(this);
        [ContextMenu("Invoke/Death")]
        public void DeathInvoke() => Death.Invoke(this);
    }
}