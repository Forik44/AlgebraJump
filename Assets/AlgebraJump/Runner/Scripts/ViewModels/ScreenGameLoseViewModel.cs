using System;
using Lukomor.Reactive;

namespace AlgebraJump.Runner
{
    public class ScreenGameLoseViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> GameLoseText => _loserText;
        public IReactiveProperty<string> ResultText => _resultText;

        private readonly SingleReactiveProperty<string> _loserText = new();
        private readonly SingleReactiveProperty<string> _resultText = new();

        private readonly GameSessionService _gameSessionsService;
        private readonly ScenesService _scenesService;
        private readonly Action _openGameplayScreen;

        public ScreenGameLoseViewModel(GameSessionService gameSessionsService, ScenesService scenesService, Action openGameplayScreen)
        {
            _gameSessionsService = gameSessionsService;
            _scenesService = scenesService;
            _openGameplayScreen = openGameplayScreen;
            
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
            _resultText.Value = $"{gameSessionsService.PlayerScore.Value} / {gameSessionsService.ScoreLimit}";
        }
        
        public void HandleAgainButtonClick()
        {
            _gameSessionsService.RestartGame();
            _openGameplayScreen();
        }

        public void HandleMainMenuButtonClick()
        {
            _scenesService.LoadMainMenuScene();
        }
    }
}
