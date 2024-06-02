using System;

namespace AlgebraJump.Bank
{
    public interface IReadOnlyPlayerResource
    {
        event Action<int> AmountChanged;
        
        ResourceType PlayerResourceType { get; }
        int Amount { get; }
        bool IsEmpty { get; }
    }
}