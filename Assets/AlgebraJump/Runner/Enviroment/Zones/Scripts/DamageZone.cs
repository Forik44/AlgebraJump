using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class DamageZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, PlayerView player)
        {
            gameSessionService.LoseGame();
        }

        public override void Exit(GameSessionService gameSessionService, PlayerView player)
        {

        }
    }
}