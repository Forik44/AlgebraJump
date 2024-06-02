using System;
using Lukomor.MVVM;
using Lukomor.Reactive;

namespace AlgebraJump.Runner
{
    public class UIRootGameplayViewModel : IViewModel
    {
        public IReactiveProperty<ScreenViewModel> OpenedScreen => _openedScreen;

        private readonly ReactiveProperty<ScreenViewModel> _openedScreen = new();
        private readonly Func<ScreenPauseViewModel> _screenPauseFactory;
        private readonly Func<ScreenGameLoseViewModel> _screenGameLoseFactory;
        private readonly Func<ScreenGameWinViewModel> _screenGameWinFactory;
        private readonly Func<ScreenGameplayViewModel> _screenGameplayFactory;
        private readonly GameSessionService _gameSessionsService;

        public UIRootGameplayViewModel(
            Func<ScreenPauseViewModel> screenPauseFactory,
            Func<ScreenGameLoseViewModel> screenGameLoseFactory,
            Func<ScreenGameWinViewModel> screenRoundWinFactory,
            Func<ScreenGameplayViewModel> screenGameplayFactory,
            GameSessionService gameSessionsService)
        {
            _screenPauseFactory = screenPauseFactory;
            _screenGameLoseFactory = screenGameLoseFactory;
            _screenGameWinFactory = screenRoundWinFactory;
            _screenGameplayFactory = screenGameplayFactory;
            _gameSessionsService = gameSessionsService;

            gameSessionsService.GameLose.Subscribe(_ => OpenGameLoseScreen());
            gameSessionsService.GameWin.Subscribe(_ => OpenGameWinScreen());
        }

        public void OpenGameplayScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGameplayFactory();
        }

        public void HandlePauseButtonClick()
        {
            if (_gameSessionsService.IsPaused.Value)
            {
                _gameSessionsService.Unpause();
                OpenGameplayScreen();
            }
            else
            {
                _gameSessionsService.Pause();
                OpenPauseScreen();
            }
        }
        
        private void OpenPauseScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenPauseFactory();
        }

        private void OpenGameLoseScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGameLoseFactory();
        }

        private void OpenGameWinScreen()
        {
            CloseOldScreen();

            _openedScreen.Value = _screenGameWinFactory();
        }

        private void CloseOldScreen()
        {
            _openedScreen.Value?.Close();
        }
    }
}
