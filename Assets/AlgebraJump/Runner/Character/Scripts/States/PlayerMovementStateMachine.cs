namespace Lukomor.AlgebraJump.Runner
{
    public class PlayerMovementStateMachine : StateMachine
    {
        private RunMovementState _running;
        private JumpMovementState _jumping;
        private FlyMovementState _flying;
        private DieMovementState _dying;

        public PlayerMovementStateMachine(PlayerView player, PlayerResources playerResources)
        {
            _running = new RunMovementState(player, playerResources);
            _jumping = new JumpMovementState(player, playerResources);
            _flying = new FlyMovementState(player, playerResources);
            _dying = new DieMovementState(player, playerResources);
            
            Initialize(_running);
        }

        public void SetRunningState()
        {
            ChangeState(_running);
        }
        
        public void SetJumpState()
        {
            ChangeState(_jumping);
        }
        
        public void SetFlyingState()
        {
            ChangeState(_flying);
        }
        
        public void SetDyingState()
        {
            ChangeState(_dying);
        }

        public void RestartMachine()
        {
            ChangeState(_running);
        }

        public bool IsFlying()
        {
            return CurrentState == _flying;
        }
    }
}