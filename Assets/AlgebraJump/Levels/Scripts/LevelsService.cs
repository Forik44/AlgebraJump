using AlgebraJump.Runner;

namespace AlgebraJump.Levels
{
    public class LevelsService
    {
        private readonly LevelsData _levelsData;
        private readonly ScenesService _scenesService;

        public LevelsService(LevelsData levelsData, ScenesService scenesService)
        {
            _levelsData = levelsData;
            _scenesService = scenesService;

            _scenesService.StartLoadGameplayScene += LoadLevel;
        }

        private void LoadLevel(string levelId)
        {
            _scenesService.LoadLevel(_levelsData.Levels[levelId].SceneName);
        }
    }
}