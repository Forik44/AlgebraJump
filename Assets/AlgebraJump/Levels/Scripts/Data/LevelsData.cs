using System;
using System.Collections.Generic;
using AlgebraJump.UnityUtils;
using UnityEngine;

namespace AlgebraJump.Levels
{
    [Serializable]
    public class LevelsData
    {
        public SerializableDictionary<string, LevelData> Levels;
    }
}