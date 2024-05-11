using System;
using Lukomor.Reactive;

namespace Lukomor.AlgebraJump.Runner
{
    public class ScreenGameWinViewModel : ScreenViewModel
    {
        public IReactiveProperty<string> GameWinText => _winnerText;

        private readonly SingleReactiveProperty<string> _winnerText = new();

        private readonly GameSessionService _gameSessionsService;
        private readonly ScenesService _scenesService;

        public ScreenGameWinViewModel(GameSessionService gameSessionsService, ScenesService scenesService)
        {
            _gameSessionsService = gameSessionsService;
            _scenesService = scenesService;
            
            gameSessionsService.GameLose.Subscribe(_ =>
            {
                UpdateText();
            });
            
            UpdateText();
        }

        private void UpdateText()
        {
            _winnerText.Value = $"You Win!";
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