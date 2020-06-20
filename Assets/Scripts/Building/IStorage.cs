using IGDSS20.Enums;
using System.Collections.Generic;

namespace IGDSS20.Buildings
{
    public interface IStorage
    {
        void AddResource(ResourceType res, int count);
        bool GetResourceIfAvailable(ResourceType resource, int quantity);
        List<bool> GetResourcesIfAvailable(List<ResourceType> inputResources, List<int> quantities = null);

    }
}