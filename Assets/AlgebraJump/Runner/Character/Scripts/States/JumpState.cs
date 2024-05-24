using Unity.VisualScripting;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class JumpState : PlayerState
    {
        private bool _isGrounded = false;
        public JumpState(PlayerView player, StateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {

        }
        
        public override void Enter()
        {
            Debug.Log("Enter JumpState");
            if (_player.CanJump())
            {
                _player.Jump();
            }
            
            _isGrounded = false;
        }

        public override void HandleInput()
        {
            
        }

        public override void LogicUpdate()
        {
            if (TryStartFly)
            {
                _stateMachine.ChangeState(_player.Flying);
                TryStartFly = false;
                return;
            }

            if (_isGrounded)
            {
                _stateMachine.ChangeState(_player.Running);
                return;
            }

            if (TryJump)
            {
                _stateMachine.ChangeState(_player.Jumping);
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
        }
    }
}