using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Claims;
using Lukomor.Reactive;

namespace Lukomor.AlgebraJump.Runner
{
    public class GameSessionService
    {
        public IObservable<Unit> GameLose { get; }
        public IObservable<Unit> GameWin { get; }
        public IObservable<Unit> GameRestarted { get; }
        public IReactiveProperty<int> PlayerScore => _playerScore;
        public IReactiveProperty<bool> IsPaused => _isPaused;
        
        public int ScoreLimit => _scoreLimit;
        
        private readonly int _scoreLimit;
        private readonly int _startPointX;
        private readonly int _finishPointX;

        private readonly ReactiveProperty<int> _playerScore;

        private readonly ReactiveProperty<bool> _isPaused;
        private event Action<Unit> _gameLose;
        private event Action<Unit> _gameWin;
        private event Action<Unit> _restartedGame;
        
        public GameSessionService(float startPointX, float finishPointX)
        {
            _startPointX = (int)Math.Round(startPointX);
            _finishPointX = (int)Math.Round(finishPointX);
            
            _scoreLimit = WorldToRelativePosition(_finishPointX, _startPointX);
            
            _playerScore = new ReactiveProperty<int>(0);
            _isPaused = new ReactiveProperty<bool>(false);

            GameLose = Observable.FromEvent<Unit>(a => _gameLose += a, a => _gameLose -= a);
            GameWin = Observable.FromEvent<Unit>(a => _gameWin += a, a => _gameWin -= a);
            GameRestarted = Observable.FromEvent<Unit>(a => _restartedGame += a, a => _restartedGame -= a);
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
                Pause();
                _gameWin?.Invoke(Unit.Default);
            }
        }

        private int WorldToRelativePosition(float playerPosition, int startPointX)
        {
            return (int)Math.Round(playerPosition - startPointX);
        }

        public void RestartGame()
        {
            _playerScore.Value = 0;

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
        }

        public void Unpause()
        {
            _isPaused.Value = false;
        }
    }
}
