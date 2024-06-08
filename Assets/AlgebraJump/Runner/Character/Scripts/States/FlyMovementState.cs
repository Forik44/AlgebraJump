using System;
using AlgebraJump.UnityUtils;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class FlyMovementState : CharacterMovementState
    {
        private bool _upPressed;
        public FlyMovementState(Character player, PlayerResources playerResources, IEventManager eventManager) : base(player, playerResources, eventManager)
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
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
            base.Exit();
            Debug.Log("Exit FlyState");
        }
    }
}