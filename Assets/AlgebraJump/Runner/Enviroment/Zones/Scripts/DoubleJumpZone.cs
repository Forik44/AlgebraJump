namespace Lukomor.AlgebraJump.Runner
{
    public class DoubleJumpZone : IZone
    {
        public override void Enter(GameSessionService gameSessionService, PlayerView player)
        {
              player.SetDoubleJump(true);
        }

        public override void Exit(GameSessionService gameSessionService, PlayerView player)
        {
            player.SetDoubleJump(false);
        }
    }
}