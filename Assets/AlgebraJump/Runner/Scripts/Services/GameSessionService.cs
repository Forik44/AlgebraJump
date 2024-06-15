using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Claims;
using AlgebraJump.Bank;
using Lukomor.Reactive;

namespace AlgebraJump.Runner
{
    public class GameSessionService
    {
        public IObservable<Unit> GameLose { get; }
        public IObservable<Unit> GameWin { get; }
        public IObservable<Unit> GameRestarted { get; }
        public IObservable<Unit> PausedGame { get; }
        public IObservable<Unit> UnpausedGame { get; }
        public IReactiveProperty<int> PlayerScore => _playerScore;
        public IReactiveProperty<bool> IsPaused => _isPaused;
        
        public int ScoreLimit => _scoreLimit;

        private Dictionary<ResourceType, int> _collectedResources;
        private readonly int _scoreLimit;
        private readonly int _startPointX;
        private readonly int _finishPointX;
        private readonly BankService _bankService;

        private readonly ReactiveProperty<int> _playerScore;

        private readonly ReactiveProperty<bool> _isPaused;
        private event Action<Unit> _gameLose;
        private event Action<Unit> _gameWin;
        private event Action<Unit> _restartedGame;
        private event Action<Unit> _pausedGame;
        private event Action<Unit> _unpausedGame;
        
        public GameSessionService(BankService bankService, float startPointX, float finishPointX)
        {
            _bankService = bankService;
            _startPointX = (int)Math.Round(startPointX);
            _finishPointX = (int)Math.Round(finishPointX);
            
            _scoreLimit = WorldToRelativePosition(_finishPointX, _startPointX);
            
            _playerScore = new ReactiveProperty<int>(0);
            _isPaused = new ReactiveProperty<bool>(false);
            _collectedResources = new Dictionary<ResourceType, int>();

            GameLose = Observable.FromEvent<Unit>(a => _gameLose += a, a => _gameLose -= a);
            GameWin = Observable.FromEvent<Unit>(a => _gameWin += a, a => _gameWin -= a);
            GameRestarted = Observable.FromEvent<Unit>(a => _restartedGame += a, a => _restartedGame -= a);
            PausedGame = Observable.FromEvent<Unit>(a => _pausedGame += a, a => _pausedGame -= a);
            UnpausedGame = Observable.FromEvent<Unit>(a => _unpausedGame += a, a => _unpausedGame -= a);
        }

        public void UpdateScore(float playerPosition)
        {
            if (_isPaused.Value)
            {
                return;
            }

            _playerScore.Value = Math.Clamp(WorldToRelativePosition(playerPosition, _startPointX), 0, _scoreLimit);
            
            if (_playerScore.Value >= _scoreLimit)
            {
                WinGame();
            }
        }

        public void RestartGame()
        {
            _playerScore.Value = 0;
            _collectedResources.Clear();
            
            Unpause();
            
            _restartedGame?.Invoke(Unit.Default);
        }
        
        public void LoseGame()
        {
            Pause();
            
            _gameLose?.Invoke(Unit.Default);
        }
        
        public void Pause()
        {
            _isPaused.Value = true;
            _pausedGame?.Invoke(Unit.Default);
        }

        public void Unpause()
        {
            _isPaused.Value = false;
            _unpausedGame?.Invoke(Unit.Default);
        }

        public void AddCollectedResource(ResourceType resourceType, int amount)
        {
            if (_collectedResources.ContainsKey(resourceType))
            {
                _collectedResources[resourceType] += amount;
            }
            else
            {
                _collectedResources[resourceType] = amount;
            }
        }
        
        private void WinGame()
        {
            Pause();

            SaveResult();

            _gameWin?.Invoke(Unit.Default);
        }

        private void SaveResult()
        {
            foreach (var resource in _collectedResources)
            {
                _bankService.AddItems(resource.Key, resource.Value);
            }
        }

        private int WorldToRelativePosition(float playerPosition, int startPointX)
        {
            return (int)Math.Round(playerPosition - startPointX);
        }
    }
}
