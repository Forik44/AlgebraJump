using System;
using AlgebraJump.Bank;
using AlgebraJump.Levels;
using Unity.VisualScripting;

namespace AlgebraJump
{
    [Serializable]
    public class GameStateData
    {
        public BankData BankData;
        public LevelsData LevelsData;
    }
}