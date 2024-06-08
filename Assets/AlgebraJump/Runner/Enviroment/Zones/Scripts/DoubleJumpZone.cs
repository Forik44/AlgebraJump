using AlgebraJump.Bank;

namespace AlgebraJump.Runner
{
    public class DoubleJumpZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, ICharacter player)
        {
            player.SetDoubleJump(true);
        }

        public override void Exit(GameSessionService gameSessionService, ICharacter player)
        {
            player.SetDoubleJump(false);
        }
    }
}