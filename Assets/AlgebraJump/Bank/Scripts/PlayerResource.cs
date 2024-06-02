using System;
using TMPro;

namespace AlgebraJump.Bank
{
    public class PlayerResource : IReadOnlyPlayerResource
    {
        public event Action<int> AmountChanged;
        
        public ResourceType PlayerResourceType => _data.PlayerResourceType;

        public int Amount
        {
            get => _data.Amount;
            set
            {
                if (_data.Amount != value)
                {
                    _data.Amount = value;
                    AmountChanged?.Invoke(value);
                }
            }
        }

        public bool IsEmpty => Amount == 0;
        
        private readonly PlayerResourceData _data;
        
        public PlayerResource(PlayerResourceData data)
        {
            _data = data;
        }
    }
}