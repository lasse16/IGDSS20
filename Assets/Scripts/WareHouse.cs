using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class WareHouse : MonoBehaviour
    {
        private Dictionary<ResourceType, int> StockPile;

        private void Start()
        {
            StockPile = new Dictionary<ResourceType, int>();
        }

        internal List<bool> GetResourcesIfAvailable(List<ResourceType> inputResources)
        {
            var resourceAvailable = new List<bool>();

            var multipleResourceLock = new object();
            lock (multipleResourceLock)
            {
                foreach (var item in inputResources)
                {
                    resourceAvailable.Add(GetResourceIfAvailable(item, 1));
                }
            }

            return resourceAvailable;
        }

        public bool GetResourceIfAvailable(ResourceType resource, int quantity)
        {
            //lock for multithreaded access
            var resourceLock = new object();
            lock (resourceLock)
            {
                if (StockPile[resource] <= quantity)
                {
                    return false;
                }

                StockPile[resource] -= quantity;
                return true;
            }
        }

        public void AddResource(ResourceType res, int count)
        {
            count = count < 0 ? throw new ArgumentException() : count;

            StockPile[res] += count;
        }

    }
}
