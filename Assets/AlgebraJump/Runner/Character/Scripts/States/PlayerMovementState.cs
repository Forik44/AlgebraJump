using Unity.VisualScripting;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public abstract class PlayerMovementState : State
    {
        //TODO: Придумать что сделать с паблик статик, выглядит разъёбно
        public static bool TryStartFly = false;
        public static bool TryStopFly = false;
        public static bool TryJump = false;

        protected readonly PlayerView _player;
        protected readonly PlayerMovementStateMachine _stateMachine;
        protected readonly PlayerResources _playerResources;

        protected PlayerMovementState(PlayerView player, PlayerMovementStateMachine stateMachine, PlayerResources playerResources)
        {
            _player = player;
            _stateMachine = stateMachine;
            _playerResources = playerResources;
        }

        public override void Enter() { }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump = true;
            }
        }
        public override void LogicUpdate() { }

        public override void PhysicsUpdate() { }

        public override void Exit() { }

        public void SetDieState()
        {
            _stateMachine.SetDyingState();
        }

        protected void Move(Vector3 direction)
        {
            if (!_player.IsActive)
            {
                return;
            }
            
            var nextPosition = Vector3.Lerp(_player.transform.position, _player.transform.position + direction * _playerResources.Speed, Time.deltaTime * _playerResources.Smoothing);

            _player.SetPosition(nextPosition);
        }
    }
}