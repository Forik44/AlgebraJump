using System;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.AlgebraJump.Runner
{
    public class StartPointViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }

        public StartPointViewModel(GameSessionService gameSessionService)
        {
            IsActive = gameSessionService.IsPaused.Select(value => !value);
        }
    }
}