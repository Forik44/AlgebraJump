using System.Collections;
using System.Collections.Generic;
using AlgebraJump.Bank;
using UnityEditor;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public abstract class IZone : MonoBehaviour
    {
        public abstract void Enter(GameSessionService gameSessionService, ICharacter player);
        public abstract void Exit(GameSessionService gameSessionService, ICharacter player);
    }
}