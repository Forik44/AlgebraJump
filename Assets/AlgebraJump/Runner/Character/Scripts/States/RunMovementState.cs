using AlgebraJump.UnityUtils;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class RunMovementState : CharacterMovementState
    {
        private bool _isGrounded = false;
        public RunMovementState(Character player, PlayerResources playerResources, IEventManager eventManager) : base(player, playerResources, eventManager)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter RunState");
            _player.SetRunAnimation();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            if (_player.TryStartFly)
            {
                _player.MovementStateMachine.SetFlyingState();
                return;
            }
            
            if (_player.TryJump)
            { 
                _player.MovementStateMachine.SetJumpState();
                _player.TryJump = false;
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
            base.Exit();
            Debug.Log("Exit RunState");
        }
    }
}