using System;
using AlgebraJump.UnityUtils;
using Unity.VisualScripting;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public abstract class CharacterMovementState : State, IDisposable
    {
        protected readonly Character _player;
        protected readonly PlayerResources _playerResources;
        
        protected CharacterMovementState(Character player, PlayerResources playerResources, IEventManager eventManager) : base(eventManager)
        {
            _player = player;
            _playerResources = playerResources;
        }

        public override void HandleInput()
        {
            //TODO:Перенести
            if (Input.GetKey(KeyCode.Space))
            {
                _player.TryJump = true;
            }
        }
        public override void LogicUpdate() { }

        public override void PhysicsUpdate() { }

        protected void Move(Vector3 direction)
        {
            if (!_player.IsActive)
            {
                return;
            }
            
            var nextPosition = Vector3.Lerp(_player.Transform.position, _player.Transform.position + direction * _playerResources.Speed, Time.deltaTime * _playerResources.Smoothing);

            _player.SetPosition(nextPosition);
        }
    }
}