using System;
using System.Collections.Generic;

namespace AlgebraJump.Bank
{
    public interface IReadOnlyBank
    {
        event Action<ResourceType, int> ResourceAdded;
        event Action<ResourceType, int> ResourceRemoved;

        int GetAmount(ResourceType resource);
        bool Has(ResourceType resource, int amount);
        List<PlayerResourceData> GetResources();
    }
}