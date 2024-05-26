using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class RunMovementState : PlayerMovementState
    {
        private bool _isGrounded = false;
        public RunMovementState(PlayerView player, PlayerMovementStateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {
            
        }
        
        public override void Enter()
        {
            Debug.Log("Enter RunState");
            _player.SetRunAnimation();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            if (TryStartFly)
            {
                _stateMachine.SetFlyingState();
                return;
            }
            
            if (TryJump)
            { 
                _stateMachine.SetJumpState();
                TryJump = false;
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            _isGrounded = _player.IsOnTheGround();
            
            if (_isGrounded && _player.CheckNegativeVelocity())
            {
                _player.SetAbsVelocity(-2);
            }
            
            Move(Vector3.right);
        }

        public override void Exit()
        {
            Debug.Log("Exit RunState");
        }
    }
}