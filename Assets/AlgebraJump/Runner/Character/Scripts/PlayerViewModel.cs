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

        private readonly GameSessionService _gameSessionService;
        private readonly PlayerView _player;


        public PlayerViewModel(GameSessionService gameSessionService, PlayerView player)
        {
            _gameSessionService = gameSessionService;
            _player = player;

            IsActive = _gameSessionService.IsPaused.Select(value => !value);
            _gameSessionService.PausedGame.Subscribe(_ =>
            {
                StopMoving();
            }); 
            _gameSessionService.UnpausedGame.Subscribe(_ =>
            {
                StartMoving();
            });
            
            PositionReset = gameSessionService.GameRestarted.Merge(gameSessionService.GameRestarted);

            _player.OnTriggerEnter += OnTriggerEnter;
            _player.OnTriggerExit += OnTriggerExit;
        }
        
        public void UpdatePlayerPosition()
        {
            var playerPosition = (int)Math.Round(_player.transform.position.x);
            
            _gameSessionService.UpdateScore(playerPosition);
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
            if (!other.TryGetComponent(out IZone zone))
            {
                return;
            }
            
            zone.Enter(_gameSessionService, _player);
        }
        
        private void OnTriggerExit(Collider2D other)
        {
            if (!other.TryGetComponent(out IZone zone))
            {
                return;
            }

            zone.Exit(_gameSessionService, _player);
        }
    }
}
