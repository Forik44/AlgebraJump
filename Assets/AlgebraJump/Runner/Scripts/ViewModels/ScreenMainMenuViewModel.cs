using System;

namespace Lukomor.AlgebraJump.Runner
{
    public class ScreenMainMenuViewModel : ScreenViewModel
    {
        private readonly Action _startGameplay;
        
        public ScreenMainMenuViewModel(Action startGameplay)
        {
            _startGameplay = startGameplay;
        }
        
        public void StartGameButtonClicked()
        {
            _startGameplay();
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
