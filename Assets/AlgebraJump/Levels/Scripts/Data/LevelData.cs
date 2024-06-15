using System;
using System.Collections.Generic;
using AlgebraJump.UnityUtils;

namespace AlgebraJump.Levels
{
    [Serializable]
    public class LevelData
    {
        public string SceneName;
        public List<CollectedZoneData> CollectedResourceInLevels;
    }
}