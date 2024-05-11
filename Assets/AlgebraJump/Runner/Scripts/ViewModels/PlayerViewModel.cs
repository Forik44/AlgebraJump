using System;
using System.Reactive;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.AlgebraJump.Runner
{
    public class PlayerViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }
        public IObservable<Unit> PositionReset { get; }
        
        private readonly GameSessionService _gameSessionsService;

        public PlayerViewModel(GameSessionService gameSessionsService)
        {
            _gameSessionsService = gameSessionsService;

            IsActive = _gameSessionsService.IsPaused.Select(value => !value);
        }
    }
}
