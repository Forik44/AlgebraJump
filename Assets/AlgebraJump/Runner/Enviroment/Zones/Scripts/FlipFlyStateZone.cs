using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class FlipFlyStateZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, PlayerView player)
        {
            player.FlipFlyState();
        }

        public override void Exit(GameSessionService gameSessionService, PlayerView player)
        {
            player.FlipFlyState();
        }
    }
}