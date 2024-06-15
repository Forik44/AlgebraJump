using System;

namespace AlgebraJump.Runner
{
    public class ScreenMainMenuViewModel : ScreenViewModel
    {
        private readonly Action<string> _startGameplay;
        
        public ScreenMainMenuViewModel(Action<string> startGameplay)
        {
            _startGameplay = startGameplay;
        }
        
        public void StartGameButtonClicked()
        {
            _startGameplay("FirstLevel");
        }

        public void ExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
