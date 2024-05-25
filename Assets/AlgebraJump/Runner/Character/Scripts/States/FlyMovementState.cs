using System;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class FlyMovementState : PlayerMovementState
    {
        private bool _upPressed;
        public FlyMovementState(PlayerView player, PlayerMovementStateMachine stateMachine, PlayerResources playerResources) : base(player, stateMachine, playerResources)
        {
            
        }
        
        public override void Enter()
        {
            Debug.Log("Enter FlyState");
            _player.SetFlyAnimation(true);
        }

        public override void HandleInput()
        {
            _upPressed = Input.GetKey(KeyCode.Space);
        }

        public override void LogicUpdate()
        {
            if (TryStopFly)
            {
                _stateMachine.SetRunningState();
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
            _player.SetFlyAnimation(false);
        }
    }
}