using System;
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
        [SerializeField] private WorkerEvent ComingOfAge;
        [SerializeField] private WorkerEvent Retiring;
        [SerializeField] private WorkerEvent Death;

        public HousingBuilding Home;

        [SerializeField] private List<ResourceType> RequiredResources;
        [Range(0, float.MaxValue)]
        [SerializeField] private int _age;
        [Range(0, float.MaxValue)]
        [SerializeField] private float _happiness;

        private const float ageIntervalInSeconds = 15;
        private float timeSinceLastAgeIncrease;

        private const float resourceRequestIntervalInSeconds = 60;
        private float timeSinceLastResourceRequest;


        private HashSet<ResourceType> _missingResources;
        private void Awake()
        {
            RequestAllResources();

        }

        private void Update()
        {
            timeSinceLastAgeIncrease += Time.deltaTime;
            timeSinceLastResourceRequest += Time.deltaTime;

            //TODO Extract time based method call
            //Maybe check out InvokeRepeating()
            if (timeSinceLastAgeIncrease > ageIntervalInSeconds)
            {
                _age++;
                timeSinceLastAgeIncrease -= ageIntervalInSeconds;
                CheckAge(_age);
            }

            if (timeSinceLastResourceRequest > resourceRequestIntervalInSeconds)
            {
                timeSinceLastResourceRequest -= resourceRequestIntervalInSeconds;
                RequestAllResources();
            }

            CalculateRessourceHappiness(_missingResources.ToList());
        }

        private void RequestAllResources()
        {
            _missingResources = new HashSet<ResourceType>(RequiredResources);
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

        //TODO Extract Happiness calculator
        //Move this out and include a list with ratio, so happiness can be made up of different components,
        //like employment, decorations etc.
        private void CalculateRessourceHappiness(List<ResourceType> resources)
        {
            foreach (var item in resources)
            {
                var happinessChange = 1 / RequiredResources.Count;
                var available = Home.WareHouse.GetResourceIfAvailable(item, 1);
                if (available)
                {
                    _missingResources.Remove(item);
                    _happiness += happinessChange;
                }
                else
                {
                    if (_missingResources.Add(item))
                    {
                        //Only decrease happiness if resource was not missing prior
                        _happiness -= happinessChange;
                    }
                }
            }
        }
    }
}
