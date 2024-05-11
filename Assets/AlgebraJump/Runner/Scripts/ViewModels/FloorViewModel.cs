using System;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.AlgebraJump.Runner
{
    public class FloorViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }
        
        public FloorViewModel(GameSessionService gameSessionService)
        {
            IsActive = gameSessionService.IsPaused.Select(value => !value);
        }
    }
}