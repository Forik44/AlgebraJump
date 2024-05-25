using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class JumpMovementState : PlayerMovementState
    {
        private bool _isGrounded = false;
        public JumpMovementState(PlayerView player, PlayerMovementStateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {

        }
        
        public override void Enter()
        {
            Debug.Log("Enter JumpState");
            if (_player.CanJump())
            {
                _player.Jump();
                _player.SetJumpAnimation(true);
            }
            else
            {
                TryJump = false;
            }
            
            _isGrounded = false;
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
                TryStartFly = false;
                return;
            }

            if (_isGrounded && _player.CheckNegativeVelocity())
            {
                _stateMachine.SetRunningState();
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
            Move(Vector3.right);
        }

        public override void Exit()
        {
            Debug.Log("Exit JumpState");
            _player.SetJumpAnimation(false);
        }
    }
}