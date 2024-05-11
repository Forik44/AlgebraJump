using System;
using System.Reactive.Linq;
using Lukomor.MVVM;

namespace Lukomor.AlgebraJump.Runner
{
    public class FinishPointViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }

        public FinishPointViewModel(GameSessionService gameSessionService)
        {
            IsActive = gameSessionService.IsPaused.Select(value => !value);
        }
    }
}