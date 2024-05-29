using Unity.VisualScripting;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public abstract class PlayerMovementState : State
    {
        protected readonly PlayerView _player;
        protected readonly PlayerResources _playerResources;

        protected PlayerMovementState(PlayerView player, PlayerResources playerResources)
        {
            _player = player;
            _playerResources = playerResources;
        }

        public override void Enter() { }

        public override void HandleInput()
        {
            //TODO:Перенести
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.TryJump = true;
            }
        }
        public override void LogicUpdate() { }

        public override void PhysicsUpdate() { }

        public override void Exit() { }

        public void SetDieState()
        {
            _player.MovementStateMachine.SetDyingState();
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