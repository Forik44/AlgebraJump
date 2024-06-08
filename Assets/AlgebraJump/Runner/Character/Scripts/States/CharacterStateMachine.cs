using AlgebraJump.UnityUtils;

namespace AlgebraJump.Runner
{
    public class CharacterStateMachine : StateMachine
    {
        private RunMovementState _running;
        private JumpMovementState _jumping;
        private FlyMovementState _flying;
        private DieMovementState _dying;

        public CharacterStateMachine(Character player, PlayerResources playerResources, IEventManager eventManager)
        {
            _running = new RunMovementState(player, playerResources, eventManager);
            _jumping = new JumpMovementState(player, playerResources, eventManager);
            _flying = new FlyMovementState(player, playerResources, eventManager);
            _dying = new DieMovementState(player, playerResources, eventManager);
            
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