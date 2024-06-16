using System;
using System.Collections.Generic;
using AlgebraJump.UnityUtils;
using UnityEngine.Serialization;

namespace AlgebraJump.Levels
{
    [Serializable]
    public class LevelData
    {
        public string SceneName;
        public List<string> CollectedResourceZonesInLevels;
    }
}