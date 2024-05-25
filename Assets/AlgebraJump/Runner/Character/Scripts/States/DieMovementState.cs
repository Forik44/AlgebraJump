using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class DieMovementState : PlayerMovementState
    {
        public DieMovementState(PlayerView player, PlayerMovementStateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {
            
        }
        
        public override void Enter()
        {
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
            Debug.Log("Exit DieState");
        }
    }
}