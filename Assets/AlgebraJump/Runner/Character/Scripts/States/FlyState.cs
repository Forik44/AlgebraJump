using System;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class FlyState : PlayerState
    {
        private bool _upPressed;
        public FlyState(PlayerView player, StateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {
            
        }
        
        public override void Enter()
        {
            Debug.Log("Enter FlyState");
        }

        public override void HandleInput()
        {
            _upPressed = Input.GetKey(KeyCode.Space);
        }

        public override void LogicUpdate()
        {
            if (TryStopFly)
            {
                _stateMachine.ChangeState(_player.Running);
                TryStopFly = false;
                return;
            }
        }
        

        public override void PhysicsUpdate()
        {
            if (_upPressed)
            {
                _player.LerpFlyVelocity();
            }

            Move(Vector3.right);
        }

        public override void Exit()
        {
            Debug.Log("Exit FlyState");
        }
    }
}