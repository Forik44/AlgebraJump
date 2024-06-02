using System.Collections.Generic;
using AlgebraJump.Bank;
using UnityEngine;

namespace AlgebraJump.Scripts
{
    public class GameStatePlayerProvider : IGameStateProvider, IGameStateSaver
    {
        private const string KEY = "GAME STATE";
        
        public GameStateData GameState { get; private set; }
        
        public void SaveGameState()
        {
            var json = JsonUtility.ToJson(GameState);
            PlayerPrefs.SetString(KEY, json);
        }

        public void LoadGameState()
        {
            if (PlayerPrefs.HasKey(KEY))
            {
                var json = PlayerPrefs.GetString(KEY);
                GameState = JsonUtility.FromJson<GameStateData>(json);
            }
            else
            {
                GameState = InitFromSettings();
                SaveGameState();
            }
        }

        private GameStateData InitFromSettings()
        {
            //TODO: Добавить загрузку начальных данных из конфига
            var gameState = new GameStateData
            {
                BankData = CreateBankData()
            };

            return gameState;
        }

        private BankData CreateBankData()
        {
            var coinsData = new PlayerResourceData();
            coinsData.PlayerResourceType = ResourceType.Coins;
            coinsData.Amount = 0;

            var bankData = new BankData();
            bankData.PlayerResources = new List<PlayerResourceData>();
            bankData.PlayerResources.Add(coinsData);
            
            return bankData;
        }
    }
}