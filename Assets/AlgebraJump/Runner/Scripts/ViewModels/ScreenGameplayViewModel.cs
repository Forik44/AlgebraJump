using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace Lukomor.AlgebraJump.Runner
{
    public class ScreenGameplayViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> GameScore => _gameScore;
        
        private readonly SingleReactiveProperty<string> _gameScore = new();

        public ScreenGameplayViewModel(GameSessionService gameSessionsService)
        {
            gameSessionsService.PlayerScore.Subscribe(_ =>
            {
                _gameScore.Value = $"{gameSessionsService.PlayerScore.Value}:{gameSessionsService.ScoreLimit}";
            });
        }
    }
}
