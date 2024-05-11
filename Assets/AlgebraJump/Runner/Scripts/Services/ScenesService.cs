using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lukomor.AlgebraJump.Runner
{
    public class ScenesService
    {
        public const string SCENE_GAMEPLAY = "AlgebraJumpGameplay";
        public const string SCENE_MAIN_MENU = "AlgebraJumpMainMenu";
        public const string SCENE_BOOT = "AlgebraJumpBoot";

        private AsyncOperation _cachedAsyncOperation;

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        public void LoadGameplayScene()
        {
            SceneManager.LoadScene(SCENE_GAMEPLAY);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(SCENE_MAIN_MENU);
        }
    }
}
