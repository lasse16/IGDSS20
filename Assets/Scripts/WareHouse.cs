using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
    public class WareHouse : MonoBehaviour
    {
        private Dictionary<ResourceType, int> StockPile;

        private void Start()
        {
            StockPile = new Dictionary<ResourceType, int>();
        }

        internal List<bool> GetResourcesIfAvailable(List<ResourceType> inputResources, List<int> quantities = null)
        {
            if (quantities is null)
                quantities = inputResources.Select(item => 1).ToList();


            if (inputResources.Count != quantities.Count)
                throw new ArgumentException($"Length of required resources and quantites are not equal: resources {inputResources.Count} quantities {quantities.Count}");

            var resourceAvailable = new List<bool>();

            var multipleResourceLock = new object();
            lock (multipleResourceLock)
            {
                for (int i = 0; i < inputResources.Count; i++)
                {
                    var item = inputResources[i];
                    var requiredQuantity = quantities[i];

                    resourceAvailable.Add(GetResourceIfAvailable(item, requiredQuantity));
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
            if(count < 0)
                throw new ArgumentException($"Resource addition failed - resource{Enum.GetName(typeof(ResourceType),res)} count {count}");

            StockPile[res] += count;
        }

    }
}
