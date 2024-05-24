using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class FlipGravityZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, PlayerView player)
        {
            player.FlipGravity();
        }

        public override void Exit(GameSessionService gameSessionService, PlayerView player)
        {
            
        }
    }
}