namespace AlgebraJump.Runner
{
    public abstract class State
    {
        public abstract void Enter();

        public virtual void HandleInput() { }
        public abstract void LogicUpdate();
        public virtual void PhysicsUpdate() { }
        
        public abstract void Exit();
    }
}