using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class RunState : PlayerState
    {
        private bool _isGrounded = false;
        public RunState(PlayerView player, StateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {
            
        }
        
        public override void Enter()
        {
            Debug.Log("Enter RunState");
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            if (TryStartFly)
            {
                _stateMachine.ChangeState(_player.Flying);
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