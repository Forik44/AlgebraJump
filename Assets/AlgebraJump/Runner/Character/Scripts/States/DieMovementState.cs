using AlgebraJump.UnityUtils;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class DieMovementState : CharacterMovementState
    {
        public DieMovementState(Character player, PlayerResources playerResources, IEventManager eventManager) : base(player, playerResources, eventManager)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter DieState");
            _player.SetDieAnimation();
        }

        public override void HandleInput()
        {
            
        }

        public override void LogicUpdate()
        {
            
        }
        
        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exit DieState");
        }
    }
}