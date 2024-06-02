using System;
using System.Collections.Generic;
using AlgebraJump.Scripts;
using UnityEngine;

namespace AlgebraJump.Bank
{
    public class BankService : IReadOnlyBank
    {
        public event Action<ResourceType, int> ResourceAdded;
        public event Action<ResourceType, int> ResourceRemoved;
        
        private readonly BankData _data;
        private readonly IGameStateSaver _gameStateSaver;

        public BankService(BankData data, IGameStateSaver gameStateSaver)
        {
            _data = data;
            _gameStateSaver = gameStateSaver;
        }

        public List<PlayerResourceData> GetResources()
        {
            var resources = new List<PlayerResourceData>();
            foreach (var playerResource in _data.PlayerResources)
            {
                resources.Add(playerResource);
            }

            return resources;
        }

        public int GetAmount(ResourceType resource)
        {
            foreach (var playerResource in _data.PlayerResources)
            {
                if (playerResource.PlayerResourceType == resource)
                {
                    return playerResource.Amount;
                }
            }
            
            return 0;
        }

        public bool Has(ResourceType resource, int amount)
        {
            return GetAmount(resource) >= amount;
        }

        public void AddItems(ResourceType resourceType, int amount = 1)
        {
            foreach (var playerResource in _data.PlayerResources)
            {
                if (playerResource.PlayerResourceType == resourceType)
                {
                    playerResource.Amount += amount;
                    ResourceAdded?.Invoke(resourceType,amount);
                    _gameStateSaver.SaveGameState();
                    Debug.Log($"Added to {resourceType} {amount} items, Current {resourceType} = {playerResource.Amount} items");
                    return;
                }
            }
        }
        
        public void RemoveItems(ResourceType resourceType, int amount = 1)
        {
            foreach (var playerResource in _data.PlayerResources)
            {
                if (playerResource.PlayerResourceType == resourceType)
                {
                    if (playerResource.Amount < amount)
                    {
                        throw new Exception($"{resourceType} doesn't have {amount} items");
                    }
                    
                    playerResource.Amount -= amount;
                    ResourceRemoved?.Invoke(resourceType,amount);
                    _gameStateSaver.SaveGameState();
                    Debug.Log($"Removed from {resourceType} {amount} items, Current {resourceType} = {playerResource.Amount} items");
                    return;
                }
            }
        }
    }
}