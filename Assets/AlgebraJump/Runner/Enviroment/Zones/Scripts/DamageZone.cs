using AlgebraJump.Bank;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class DamageZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, PlayerView player)
        {
            player.Die();
            gameSessionService.LoseGame();
        }

        public override void Exit(GameSessionService gameSessionService, PlayerView player)
        {

        }
    }
}