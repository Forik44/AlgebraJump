using AlgebraJump.Bank;
using AlgebraJump.Levels;
using AlgebraJump.UnityUtils;
using Lukomor.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgebraJump.Runner
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        private RunnerEntryPoint _currentRunnerEntryPoint; 
        private DIContainer _rootContainer;
        private GameStatePlayerProvider _gameStateProvider;
        private ScenesService _scenesService;
        private BankService _bankService;
        private LevelsService _levelsService;
        private IEventManager _eventManager;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            _instance = new GameEntryPoint();
            _instance.Init();
        }

        private void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            _rootContainer = new DIContainer();

            InitProviders();
            InitServices();

            var sceneName = _scenesService.GetActiveSceneName();

            LoadScene(sceneName);
        }

        private void InitProviders()
        {
            _gameStateProvider = new GameStatePlayerProvider();
            _gameStateProvider.LoadGameState();
        }

        private void InitServices()
        {
            _scenesService = _rootContainer
                .RegisterSingleton(_ => new ScenesService())
                .CreateInstance();

            _bankService = _rootContainer
                .RegisterSingleton(_ => new BankService(_gameStateProvider.GameState.BankData, _gameStateProvider))
                .CreateInstance();

            _levelsService = _rootContainer
                .RegisterSingleton(_ => new LevelsService(_gameStateProvider.GameState.LevelsData, _scenesService, _gameStateProvider))
                .CreateInstance();

            _eventManager = _rootContainer
                .RegisterSingleton(_ => new UnityEventManager())
                .CreateInstance();
            
        }

        private void LoadScene(string sceneName)
        {
            if (sceneName == ScenesService.SCENE_GAMEPLAY)
            {
                StartGameplay();
                return;
            }

            if (sceneName == ScenesService.SCENE_MAIN_MENU)
            {
                Debug.Log("StartMain Scene");
                StartMainMenu();
                return;
            }

            if (sceneName != ScenesService.SCENE_BOOT)
            {
                return;
            }

            _scenesService.LoadMainMenuScene();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var sceneName = scene.name;
            
            if (sceneName == ScenesService.SCENE_MAIN_MENU)
            {
                StartMainMenu();
                return;
            }

            if (sceneName == ScenesService.SCENE_GAMEPLAY)
            {
                StartGameplay();
            }
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            var sceneName = scene.name;

            if (sceneName == ScenesService.SCENE_GAMEPLAY)
            {
                DisposeRunner();
            }
        }
        private void StartMainMenu()
        {
            var entryPoint = Object.FindObjectOfType<MainMenuEntryPoint>();
            var mainMenuContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(mainMenuContainer);
        }

        private void StartGameplay()
        {
            _scenesService.AsyncLoadLevel.completed += OnRunnerSceneLoaded;
        }

        private void OnRunnerSceneLoaded(AsyncOperation asyncOperation)
        {
            _currentRunnerEntryPoint = Object.FindObjectOfType<RunnerEntryPoint>();
            var gameplayContainer = new DIContainer(_rootContainer);
            
            _currentRunnerEntryPoint.Process(gameplayContainer);
        }

        private void DisposeRunner()
        {
            _currentRunnerEntryPoint.Dispose();
        }
    }
}
