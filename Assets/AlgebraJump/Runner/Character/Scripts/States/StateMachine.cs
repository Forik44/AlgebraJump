namespace AlgebraJump.Runner
{
    public abstract class StateMachine
    {
        public State CurrentState { get; private set; }
        
        public void Initialize(State startingMovementState)
        {
            CurrentState = startingMovementState;
            startingMovementState.Enter();
        }

        protected void ChangeState(State newMovementState)
        {
            CurrentState.Exit();

            CurrentState = newMovementState;
            newMovementState.Enter();
        }
    }
}