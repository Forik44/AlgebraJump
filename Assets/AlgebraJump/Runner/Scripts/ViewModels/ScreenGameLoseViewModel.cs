using System;
using Lukomor.Reactive;

namespace Lukomor.AlgebraJump.Runner
{
    public class ScreenGameLoseViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> GameLoseText => _loserText;
        public IReactiveProperty<string> ResultText => _resultText;

        private readonly SingleReactiveProperty<string> _loserText = new();
        private readonly SingleReactiveProperty<string> _resultText = new();

        private readonly GameSessionService _gameSessionsService;
        private readonly ScenesService _scenesService;

        public ScreenGameLoseViewModel(GameSessionService gameSessionsService, ScenesService scenesService)
        {
            _gameSessionsService = gameSessionsService;
            _scenesService = scenesService;
            
            gameSessionsService.GameLose.Subscribe(_ =>
            {
                UpdateLoseText();
                UpdateResultText(gameSessionsService);
            });
            
            UpdateLoseText();
            UpdateResultText(gameSessionsService);
        }

        private void UpdateLoseText()
        {
            _loserText.Value = $"You Lose!";
        }
        
        private void UpdateResultText(GameSessionService gameSessionsService)
        {
            _resultText.Value = $"{gameSessionsService.PlayerScore} / {gameSessionsService.ScoreLimit}";
        }
        
        public void HandleAgainButtonClick()
        {
            _gameSessionsService.RestartGame();
        }

        public void HandleMainMenuButtonClick()
        {
            _scenesService.LoadMainMenuScene();
        }
    }
}