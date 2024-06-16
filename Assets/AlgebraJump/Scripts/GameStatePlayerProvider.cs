using System.Collections.Generic;
using AlgebraJump.Bank;
using AlgebraJump.Levels;
using UnityEngine;

namespace AlgebraJump
{
    public class GameStatePlayerProvider : IGameStateProvider, IGameStateSaver
    {
        private const string KEY = "GAME STATE";
        
        public GameStateData GameState { get; private set; }

        private LevelsDataProvider _levelsDataProvider;
        
        public void SaveGameState()
        {
            var json = JsonUtility.ToJson(GameState);
            PlayerPrefs.SetString(KEY, json);
        }

        public void LoadGameState()
        {
            _levelsDataProvider = new LevelsDataProvider();
            if (PlayerPrefs.HasKey(KEY))
            {
                var json = PlayerPrefs.GetString(KEY);
                GameState = JsonUtility.FromJson<GameStateData>(json);
                GameState.LevelsData = _levelsDataProvider.UpdateLevelsData(GameState.LevelsData);
                SaveGameState();
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
                BankData = CreateBankData(),
                LevelsData = LoadLevelsData()
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
        
        private LevelsData LoadLevelsData()
        {
            _levelsDataProvider = new LevelsDataProvider();

            return _levelsDataProvider.CreateLevelsData();
        }
    }
}