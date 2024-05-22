using System;
using System.Reactive.Linq;
using Lukomor.MVVM;
using Unity.VisualScripting;
using UnityEngine;
using Unit = System.Reactive.Unit;

namespace Lukomor.AlgebraJump.Runner
{
    public class PlayerViewModel : IViewModel
    {
        public IObservable<bool> IsActive { get; }
        public IObservable<Unit> PositionReset { get; }

        private readonly GameSessionService _gameSessionsService;
        private readonly PlayerView _player;


        public PlayerViewModel(GameSessionService gameSessionsService, PlayerView player)
        {
            _gameSessionsService = gameSessionsService;
            _player = player;

            IsActive = _gameSessionsService.IsPaused.Select(value => !value);
            _gameSessionsService.PausedGame.Subscribe(_ =>
            {
                StopMoving();
            }); 
            _gameSessionsService.UnpausedGame.Subscribe(_ =>
            {
                StartMoving();
            });
            
            PositionReset = gameSessionsService.GameRestarted.Merge(gameSessionsService.GameRestarted);

            _player.OnTriggerEnter += OnTriggerEnter;
            _player.OnTriggerExit += OnTriggerExit;
        }
        
        public void UpdatePlayerPosition()
        {
            var playerPosition = (int)Math.Round(_player.transform.position.x);
            
            _gameSessionsService.UpdateScore(playerPosition);
        }
        
        public void StopMoving()
        {
            _player.StopMoving();
        }
        
        public void StartMoving()
        {
            _player.StartMoving();
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (!other.TryGetComponent(out Zone zone))
            {
                return;
            }

            switch (zone.ZoneType)
            {
                case ZoneType.Default:
                    break;
                case ZoneType.Damage:
                    _gameSessionsService.LoseGame();
                    break;
                case ZoneType.DoubleJump:
                    _player.SetDoubleJump(true);
                    break;
                case ZoneType.GravityFlip:
                    _player.FlipGravity();
                    break;
                case ZoneType.SetFly:
                    _player.FlipFly();    
                    break;
            }
        }
        
        private void OnTriggerExit(Collider2D other)
        {
            if (!other.TryGetComponent(out Zone zone))
            {
                return;
            }

            switch (zone.ZoneType)
            {
                case ZoneType.Default:
                    break;
                case ZoneType.Damage:
                    break;
                case ZoneType.DoubleJump:
                    _player.SetDoubleJump(false);
                    break;
                case ZoneType.SetFly:
                    break;
            }
        }
    }
}
