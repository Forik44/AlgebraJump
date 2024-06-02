using System;
using System.Reactive.Linq;
using Lukomor.Reactive;

namespace AlgebraJump.Runner
{
    public class ScreenGameplayViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> GameScore => _gameScore;
        public IReactiveProperty<float> Progress => _progress;
        
        private readonly SingleReactiveProperty<string> _gameScore = new();
        private readonly SingleReactiveProperty<float> _progress = new();

        public ScreenGameplayViewModel(GameSessionService gameSessionsService)
        {
            gameSessionsService.PlayerScore.Subscribe(_ =>
            {
                _gameScore.Value = $"{gameSessionsService.PlayerScore.Value}:{gameSessionsService.ScoreLimit}";
            });
            
            gameSessionsService.PlayerScore.Subscribe(_ =>
            {
                _progress.Value = (float)gameSessionsService.PlayerScore.Value / gameSessionsService.ScoreLimit;
            });
        }
    }
}
