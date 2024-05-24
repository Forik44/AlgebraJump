using Unity.VisualScripting;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public abstract class PlayerState
    {
        //TODO: Придумать что сделать с паблик статик, выглядит разъёбно
        public static bool TryStartFly = false;
        public static bool TryStopFly = false;
        public static bool TryJump = false;

        protected readonly PlayerView _player;
        protected readonly StateMachine _stateMachine;
        protected readonly PlayerResources _playerResources;

        protected PlayerState(PlayerView player, StateMachine stateMachine, PlayerResources playerResources)
        {
            _player = player;
            _stateMachine = stateMachine;
            _playerResources = playerResources;
        }
        
        public abstract void Enter();

        public virtual void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump = true;
            }
        }
        public abstract void LogicUpdate();
        public virtual void PhysicsUpdate() { }
        public abstract void Exit();

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