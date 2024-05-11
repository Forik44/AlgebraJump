using Lukomor.DI;
using Lukomor.MVVM;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class RunnerEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerView _player;
        [SerializeField] private View _startPoint;
        [SerializeField] private View _finishPoint;
        [SerializeField] private View _rootUIView;

        public void Process(DIContainer container)
        {
            SetupPlayer();
            RegisterViewModels(container);
            BindViewModels(container);
            OpenDefaultScreen(container);
        }

        private void SetupPlayer()
        {
            SetupPlayer<RunnerPlayerInput>(_player);
        }

        private static void SetupPlayer<T>(PlayerView player) where T : RunnerInput
        {
            var inputController = player.GetComponent<RunnerInput>();

            if (inputController)
            {
                Destroy(inputController);
            }

            inputController = player.gameObject.AddComponent<T>();

            inputController.Bind(player);
        }

        private void RegisterViewModels(DIContainer container)
        {
            container.RegisterSingleton(c => new GameSessionService());
            container.RegisterSingleton(c => new PlayerViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(c => new FloorViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(c => new StartPointViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(c => new FinishPointViewModel(c.Resolve<GameSessionService>()));
            
            // UI
            
            container.RegisterSingleton(
                c => new ScreenGameplayViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(
                c => new ScreenPauseViewModel(
                    c.Resolve<GameSessionService>(), 
                    c.Resolve<ScenesService>(),
                    c.Resolve<UIRootGameplayViewModel>().OpenGameplayScreen));
            container.RegisterSingleton(
                c => new ScreenGameLoseViewModel(c.Resolve<GameSessionService>(),c.Resolve<ScenesService>()));
            container.RegisterSingleton(c => new ScreenGameWinViewModel(c.Resolve<GameSessionService>(),c.Resolve<ScenesService>()));
            
            container.RegisterSingleton(c => new UIRootGameplayViewModel(
                () => container.Resolve<ScreenPauseViewModel>(),
                () => container.Resolve<ScreenGameLoseViewModel>(),
                () => container.Resolve<ScreenGameWinViewModel>(),
                () => container.Resolve<ScreenGameplayViewModel>(),
                c.Resolve<GameSessionService>()
            ));
        }

        private void BindViewModels(DIContainer container)
        {
            _player.GetComponent<View>().Bind(container.Resolve<PlayerViewModel>());
            _startPoint.GetComponent<View>().Bind(container.Resolve<StartPointViewModel>());
            _finishPoint.GetComponent<View>().Bind(container.Resolve<FinishPointViewModel>());
            _rootUIView.Bind(container.Resolve<UIRootGameplayViewModel>());
        }

        private void OpenDefaultScreen(DIContainer container)
        {
            var uiRoot = container.Resolve<UIRootGameplayViewModel>();
            
            uiRoot.OpenGameplayScreen();
        }
    }
}