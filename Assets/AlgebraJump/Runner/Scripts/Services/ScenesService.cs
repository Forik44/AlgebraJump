using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgebraJump.Runner
{
    public class ScenesService
    {
        public Action<string> StartLoadGameplayScene;
        
        public const string SCENE_GAMEPLAY = "AlgebraJumpGameplay";
        public const string SCENE_MAIN_MENU = "AlgebraJumpMainMenu";
        public const string SCENE_BOOT = "AlgebraJumpBoot";
        
        private AsyncOperation _cachedAsyncOperation;
        public AsyncOperation AsyncLoadLevel;

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        public void LoadGameplayScene(string levelID)
        {
            SceneManager.LoadSceneAsync(SCENE_GAMEPLAY);
            StartLoadGameplayScene?.Invoke(levelID);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(SCENE_MAIN_MENU);
        }

        public void LoadLevel(string sceneName)
        {
            AsyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
