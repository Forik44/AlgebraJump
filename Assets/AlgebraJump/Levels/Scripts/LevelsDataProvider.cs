using System.Collections.Generic;
using System.Linq;
using AlgebraJump.UnityUtils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlgebraJump.Levels
{
    public class LevelsDataProvider
    {
        private LevelsConfig _levelsConfig;

        public LevelsData CreateLevelsData()
        {
            _levelsConfig = Resources.Load<LevelsConfig>("LevelsConfig");
            
            var levelsData = new LevelsData();
            levelsData.Levels = new SerializableDictionary<string, LevelData>();
            
            foreach (var level in _levelsConfig.Levels)
            {
                var levelData = new LevelData();
                levelData.SceneName = level.SceneName;
                levelData.CollectedResourceZonesInLevels = new List<string>();
                levelsData.Levels.Add(level.LevelId, levelData);
            }
            
            return levelsData;
        }
        
        public LevelsData UpdateLevelsData(LevelsData levelsData)
        {
            _levelsConfig = Resources.Load<LevelsConfig>("LevelsConfig");
            
            foreach (var levelConfig in _levelsConfig.Levels)
            {
                var levelData = new LevelData();

                bool hasLevel = levelsData.Levels.TryGetValue(levelConfig.LevelId, out levelData);

                if (hasLevel && levelData.SceneName != levelConfig.SceneName || !hasLevel)
                {
                    levelData = new LevelData();
                    InitializeLevelData(levelsData, levelData, levelConfig);
                }
            }
            
            RemoveOldlevels(levelsData);
            
            return levelsData;
        }

        private void RemoveOldlevels(LevelsData levelsData)
        {
            var levelDataIDs = new List<string>();
            foreach (var levelData in levelsData.Levels)
            {
                levelDataIDs.Add(levelData.Key);
            }

            var needToDelete = new List<string>();
            foreach (var levelDataID in levelDataIDs)
            {
                if (!_levelsConfig.Contains(levelDataID))
                {
                    needToDelete.Add(levelDataID);
                }
            }

            foreach (var levelID in needToDelete)
            {
                levelsData.Levels.Remove(levelID);
            }
        }

        private void InitializeLevelData(LevelsData levelsData, LevelData levelData, LevelConfig levelConfig)
        {
            levelData.SceneName = levelConfig.SceneName;
            levelData.CollectedResourceZonesInLevels = new List<string>();
            levelsData.Levels.Add(levelConfig.LevelId, levelData);
        }
    }
}