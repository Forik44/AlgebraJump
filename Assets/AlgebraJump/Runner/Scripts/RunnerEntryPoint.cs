using System;
using AlgebraJump.Bank;
using AlgebraJump.UnityUtils;
using Lukomor.DI;
using Lukomor.MVVM;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class RunnerEntryPoint : MonoBehaviour, IDisposable
    {
        [SerializeField] private View _rootUIView;
        [SerializeField] private SpawnerFactory _spawnerFactory;
        [SerializeField] private PlayerResources _playerResources;
        
        private StartPointView _startPoint;
        private FinishPointView _finishPoint;
        private Parallax[] _parallaxBackgrounds;
        private CharacterHierarchy _characterHierarhy;
        private Character _character;
        private CameraFollower _cameraFollower;

        public void Process(DIContainer container)
        {
            FindLevelObjects();
            SetupPlayer(container);
            SetupBackgrounds();
            RegisterServices(container);
            RegisterViewModels(container);
            BindViewModels(container);
            OpenDefaultScreen(container);
        }

        private void FindLevelObjects()
        {
            _startPoint = FindAnyObjectByType<StartPointView>();
            _finishPoint = FindAnyObjectByType<FinishPointView>();

            _parallaxBackgrounds = FindObjectsByType<Parallax>(FindObjectsSortMode.None);
        }

        private void SetupPlayer(DIContainer container)
        {
            _characterHierarhy = _spawnerFactory.SpawnPlayer();
            _cameraFollower = _spawnerFactory.SpawnCamera();
            
            _character = new Character(_characterHierarhy, _startPoint.transform, _cameraFollower, _playerResources, container.Resolve<UnityEventManager>());

            //SetupPlayer<RunnerPlayerInput>(_player);
        }

        private static void SetupPlayer<T>(CharacterHierarchy player) where T : RunnerInput
        {
            //var inputController = player.GetComponent<RunnerInput>();

            //if (inputController)
            //{
                //Destroy(inputController);
            //}

            //inputController = player.gameObject.AddComponent<T>();

            //inputController.Bind(player);
        }
        
        private void SetupBackgrounds()
        {
            foreach (var background in _parallaxBackgrounds)
            {
                background.Initialize(_cameraFollower);
            }
        }
        
        private void RegisterServices(DIContainer container)
        {
            container.RegisterSingleton(c => new GameSessionService(c.Resolve<BankService>(),_startPoint.transform.position.x, _finishPoint.transform.position.x));
        }

        private void RegisterViewModels(DIContainer container)
        {
            container.RegisterSingleton(
                c => new PlayerViewModel(c.Resolve<GameSessionService>(), _character));
            // UI
            
            container.RegisterSingleton(
                c => new ScreenGameplayViewModel(c.Resolve<GameSessionService>()));
            container.RegisterSingleton(
                c => new ScreenPauseViewModel(
                    c.Resolve<GameSessionService>(), 
                    c.Resolve<ScenesService>(),
                    c.Resolve<UIRootGameplayViewModel>().OpenGameplayScreen));
            container.RegisterSingleton(
                c => new ScreenGameLoseViewModel(c.Resolve<GameSessionService>(),
                    c.Resolve<ScenesService>(), 
                    c.Resolve<UIRootGameplayViewModel>().OpenGameplayScreen));
            container.RegisterSingleton(c => new ScreenGameWinViewModel(c.Resolve<GameSessionService>(),
                c.Resolve<ScenesService>(), 
                c.Resolve<UIRootGameplayViewModel>().OpenGameplayScreen));
            
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
            _characterHierarhy.GetComponent<View>().Bind(container.Resolve<PlayerViewModel>());
            _rootUIView.Bind(container.Resolve<UIRootGameplayViewModel>());
        }

        private void OpenDefaultScreen(DIContainer container)
        {
            var uiRoot = container.Resolve<UIRootGameplayViewModel>();
            
            uiRoot.OpenGameplayScreen();
        }

        public void Dispose()
        {
            _character?.Dispose();
        }
    }
}