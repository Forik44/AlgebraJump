using System;
using AlgebraJump.UnityUtils;

namespace AlgebraJump.Runner
{
    public abstract class State : IDisposable
    {
        protected readonly IEventManager _eventManager;
        
        private IDisposable _updateSubscribeInput;
        private IDisposable _updateSubscribeLogic;
        private IDisposable _updateSubscribePhysics;
        public State(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public virtual void Enter()
        {
            _updateSubscribeLogic = _eventManager.Subscribe(EUnityEvent.Update, LogicUpdate);
            _updateSubscribeInput = _eventManager.Subscribe(EUnityEvent.Update, HandleInput);
            _updateSubscribePhysics = _eventManager.Subscribe(EUnityEvent.FixedUpdate, PhysicsUpdate);
        }

        public virtual void HandleInput() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }

        public virtual void Exit()
        {
            ClearSubsctibes();
        }

        public void Dispose()
        {
            ClearSubsctibes();
        }
        private void ClearSubsctibes()
        {
            _updateSubscribeInput?.Dispose();
            _updateSubscribeLogic?.Dispose();
            _updateSubscribePhysics?.Dispose();

            _updateSubscribeInput = null;
            _updateSubscribeLogic = null;
            _updateSubscribePhysics = null;
        }
    }
}