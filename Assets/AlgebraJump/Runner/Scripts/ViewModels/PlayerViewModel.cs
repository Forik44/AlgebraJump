using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.AlgebraJump.Runner
{
    public class PlayerViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }

        private readonly GameSessionService _gameSessionsService;
        private readonly PlayerView _player;

        public PlayerViewModel(GameSessionService gameSessionsService, PlayerView player)
        {
            _gameSessionsService = gameSessionsService;
            _player = player;

            IsActive = _gameSessionsService.IsPaused.Select(value => !value);
        }
        
        public void UpdatePlayerPosition()
        {
            var playerPosition = (int)Math.Round(_player.transform.position.x);
            
            _gameSessionsService.UpdateScore(playerPosition);
        }
    }
}
