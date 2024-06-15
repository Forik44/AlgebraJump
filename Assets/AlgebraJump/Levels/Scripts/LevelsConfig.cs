using System.Collections.Generic;
using UnityEngine;

namespace AlgebraJump.Levels
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "LevelsConfig", order = 51)]
    public class LevelsConfig : ScriptableObject
    {
        public List<LevelConfig> Levels;

        public bool Contains(string levelID)
        {
            foreach (var level in Levels)
            {
                if (levelID == level.LevelId)
                {
                    return true;
                }
            }

            return false;
        }
    }
    
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 51)]
    public class LevelConfig : ScriptableObject
    {
        public string LevelId;
        public string SceneName;
    }
}