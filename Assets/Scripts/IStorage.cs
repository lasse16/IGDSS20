using System.Collections.Generic;

namespace Assets.Scripts
{
    public interface IStorage
    {
        void AddResource(ResourceType res, int count);
        bool GetResourceIfAvailable(ResourceType resource, int quantity);
        List<bool> GetResourcesIfAvailable(List<ResourceType> inputResources, List<int> quantities = null);

    }
}