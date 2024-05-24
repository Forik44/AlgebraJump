using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public abstract class IZone : MonoBehaviour
    {
        public abstract void Enter(GameSessionService gameSessionService, PlayerView player);
        public abstract void Exit(GameSessionService gameSessionService, PlayerView player);
    }
}