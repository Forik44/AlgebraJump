using AlgebraJump.Bank;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class FlipFlyStateZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, ICharacter player)
        {
            player.FlipFlyState();
        }

        public override void Exit(GameSessionService gameSessionService, ICharacter player)
        {
            
        }
    }
}