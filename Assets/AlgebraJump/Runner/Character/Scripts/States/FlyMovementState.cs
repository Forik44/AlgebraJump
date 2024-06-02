using System;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class FlyMovementState : PlayerMovementState
    {
        private bool _upPressed;
        public FlyMovementState(PlayerView player, PlayerResources playerResources) : base(player, playerResources)
        {
            
        }
        
        public override void Enter()
        {
            Debug.Log("Enter FlyState");
            _player.SetFlyAnimation();
        }

        public override void HandleInput()
        {
            _upPressed = Input.GetKey(KeyCode.Space);
        }

        public override void LogicUpdate()
        {
            if (_player.TryStopFly)
            {
                _player.MovementStateMachine.SetRunningState();
                _player.TryStopFly = false;
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