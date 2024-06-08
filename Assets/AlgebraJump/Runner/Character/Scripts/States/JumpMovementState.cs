using System;
using AlgebraJump.UnityUtils;
using Unity.VisualScripting;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class JumpMovementState : CharacterMovementState
    {
        private bool _isGrounded = false;
        public JumpMovementState(Character player, PlayerResources playerResources, IEventManager eventManager) : base(player, playerResources, eventManager)
        {

        }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter JumpState");
            if (_player.CanJump())
            {
                _player.Jump();
                _player.SetJumpAnimation();
            }
            else
            {
                _player.TryJump = false;
            }
            
            _isGrounded = false;
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
                _player.TryStartFly = false;
                return;
            }

            if (_isGrounded && _player.CheckNegativeVelocity())
            {
                _player.MovementStateMachine.SetRunningState();
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
            Move(Vector3.right);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exit JumpState");
        }
    }
}