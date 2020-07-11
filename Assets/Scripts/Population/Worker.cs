using IGDSS20.Buildings;
using IGDSS20.Helpers;
using IGDSS20.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Worker : MonoBehaviour
    {
        public HousingBuilding Home;
        public ProductionBuilding Workplace;
        public WorkerEvent ArrivedAtWork;

        [SerializeField][ReadOnly] private int _age;
        [SerializeField][ReadOnly] private float _happiness;


        [SerializeField] private WorkerEvent Birth;
        [SerializeField] private WorkerEvent ComingOfAge;
        [SerializeField] private WorkerEvent Retiring;
        [SerializeField] private WorkerEvent Death;

        [SerializeField] private LinearMover MovingLogic;


        private List<IHappinessRequirement> _happinessRequirements;

        private const float ageIntervalInSeconds = 1;
        private float timeSinceLastAgeIncrease;
        private float _timeSincelastShift;
        private bool _employed = false;
        private bool _atHome = true;

        public float GetHappiness() => _happiness;

        public void Kill(Worker worker)
        {
            Destroy(gameObject);
        }

        public void Employ(Job job)
        {
            Workplace = job.Workplace;
            _employed = true;
            MoveToWork(Home.Tile);
        }


        public void Setup(HousingBuilding home)
        {
            Home = home;

            _age = 0;
            _happiness = 0;
            timeSinceLastAgeIncrease = 0;
            _happinessRequirements = GetComponents<IHappinessRequirement>().ToList();
            _employed = false;
            _atHome = true;

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

            _timeSincelastShift += Time.deltaTime;
            if (_employed && _atHome &&  _timeSincelastShift > Home.HousingBuildingStats.RecoveryTimeBetweenWorkShiftsSeconds)
            {
                MoveToWork(Home.Tile);
            }
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

        public void MoveHome(Tile startPosition)
        {
            MovingLogic.Move(gameObject, startPosition, Home.potentialFieldsList);
            MovingLogic.TargetReached.AddListener(ReachedHome);
        }

        private void ReachedHome()
        {
            _timeSincelastShift = 0;
            MovingLogic.TargetReached.RemoveListener(ReachedHome);
            _atHome = true;
        }

        public void MoveToWork(Tile startPosition)
        {
            _timeSincelastShift = 0;
            MovingLogic.Move(gameObject, startPosition, Workplace.potentialFieldsList);
            MovingLogic.TargetReached.AddListener(ReachedWork);
            _atHome = false;
        }

        private void ReachedWork()
        {
            MovingLogic.TargetReached.RemoveListener(ReachedWork);
            ArrivedAtWork.Invoke(this);
        }

        public void Fire()
        {
            Workplace = null;
            _employed = false;
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
