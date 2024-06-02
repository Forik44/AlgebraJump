using AlgebraJump.Bank;
using AlgebraJump.Scripts;
using Lukomor.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgebraJump.Runner
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        private DIContainer _rootContainer;
        private GameStatePlayerProvider _gameStateProvider;
        private ScenesService _scenesService;
        private BankService _bankService;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartGame()
        {
            _instance = new GameEntryPoint();
            _instance.Init();
        }

        private void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
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

        private void StartMainMenu()
        {
            var entryPoint = Object.FindObjectOfType<MainMenuEntryPoint>();
            var mainMenuContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(mainMenuContainer);
        }

        private void StartGameplay()
        {
            var entryPoint = Object.FindObjectOfType<RunnerEntryPoint>();
            var gameplayContainer = new DIContainer(_rootContainer);
            
            entryPoint.Process(gameplayContainer);
        }
    }
}
