using System;
using AlgebraJump.UnityUtils;
using Unity.VisualScripting;
using UnityEngine;


namespace AlgebraJump.Runner
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : ICharacter, IDisposable
    {
        public event Action<float> OnPositionChanged;
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;
        
        public CharacterStateMachine MovementStateMachine { get; private set; }
        public bool TryStartFly = false;
        public bool TryStopFly = false;
        public bool TryJump = false;
        public Transform Transform => _characterHierarchy.transform;
        
        private float _gravity = -6f;
        private float _checkGroundRadius = 0.5f;
        private float _jumpHeight = 3f;
        
        private CharacterHierarchy _characterHierarchy;

        private Transform _groundCheckerPivot => _characterHierarchy.GroundCheckerPivot;
        private LayerMask _groundMask => _characterHierarchy.GroundMask;
        private Animator _animator => _characterHierarchy.Animator;
        private Transform _visualRoot => _characterHierarchy.VisualRoot;
        private Rigidbody2D _rigidbody2D => _characterHierarchy.Rigidbody;
        private Vector3 _initialPosition;
        private CameraFollower _cameraFollower;
        private PlayerResources _playerResources;
        private bool _hasDoubleJump = false;
        private IEventManager _eventManager;

        public bool IsActive
        {
            get => _characterHierarchy.enabled;
            set => _characterHierarchy.enabled = value;
        }
        
        public Character(CharacterHierarchy characterHierarchy,Transform initialPosition, CameraFollower cameraFollower, PlayerResources playerResources, IEventManager eventManager)
        {
            _characterHierarchy = characterHierarchy;
            _initialPosition = initialPosition.position;
            _cameraFollower = cameraFollower;
            _cameraFollower.RestartCamera(initialPosition);
            _playerResources = playerResources;
            _eventManager = eventManager;

            _characterHierarchy.OnTriggerEnter += OnTriggerEnter2D;
            _characterHierarchy.OnTriggerExit += OnTriggerExit2D;
            _characterHierarchy.OnRestart += Restart;
            
            InitializeStateMachine();
            Restart();
        }
        
        public void Jump()
        {
            if (!IsActive)
            {
               return;
            }

            _rigidbody2D.velocityY = (float)Math.Sqrt(_jumpHeight * -2 * _gravity) * _rigidbody2D.gravityScale;
        }

        public void StopMoving()
        {
            _characterHierarchy.enabled = false;
            _rigidbody2D.Sleep();
        }
        
        public void StartMoving()
        {
            _characterHierarchy.enabled = true;
            _rigidbody2D.WakeUp();
        }

        public void Restart()
        {
            Transform.position = _initialPosition;
            _cameraFollower.RestartCamera(Transform);
            _rigidbody2D.gravityScale = 1;
            _visualRoot.localScale = new Vector3(_visualRoot.localScale.x,MathF.Abs(_visualRoot.localScale.y),_visualRoot.localScale.z);

            TryStartFly = false;
            TryStopFly = false;
            TryJump = false;
            MovementStateMachine.RestartMachine();

            RestartAnimator();
        }

        public void SetDoubleJump(bool doubleJump)
        {
            _hasDoubleJump = doubleJump;
        }
        
        public void FlipGravity()
        {
            //TODO: зарефакторить изменение гравитации через машину состояний
            
            _rigidbody2D.velocityY = 0;
            _rigidbody2D.gravityScale *= -1;

            _visualRoot.localScale = new Vector3(_visualRoot.localScale.x,-_visualRoot.localScale.y,_visualRoot.localScale.z);

            if (Math.Abs(_rigidbody2D.gravityScale - 1) < 0.01)
            {
                _cameraFollower.SetCameraFollowMode(CameraFollowMode.Default);
            }
            else if (Math.Abs(_rigidbody2D.gravityScale + 1) < 0.01)
            {
                _cameraFollower.SetCameraFollowMode(CameraFollowMode.FlipGravity);
            }
        }
        public void FlipFlyState()
        {
            if (!MovementStateMachine.IsFlying())
            {
                StartFly();
            }
            else
            {
                StopFly();
            }
        }
        public void StartFly()
        {
            _cameraFollower.SetCameraFollowMode(CameraFollowMode.FlyState);
            TryStartFly = true;
        }
        
        public void StopFly()
        {
            _cameraFollower.SetCameraFollowMode(CameraFollowMode.Default);
            TryStopFly = true;
        }
        
        public void SetPosition(Vector3 nextPosition)
        {
            Transform.position = nextPosition;
            OnPositionChanged?.Invoke(Transform.position.x);
        }
        
        public bool IsOnTheGround()
        {
            Vector2 groundCheckerPosition = _groundCheckerPivot.position;
            
            RaycastHit2D hit = Physics2D.Raycast(groundCheckerPosition, -Vector2.up * _rigidbody2D.gravityScale, _checkGroundRadius, _groundMask);
            
            return hit.collider != null;
        }
        public bool CanJump()
        {
            return IsOnTheGround() || _hasDoubleJump;
        }

        public void SetJumpAnimation()
        {
            _animator.SetTrigger("Jump");
        }
        
        public void SetFlyAnimation()
        {
            _animator.SetTrigger("Fly");
        }
        
        public void SetDieAnimation()
        {
            _animator.SetTrigger("Die");
        }
        
        public void SetRunAnimation()
        {
            _animator.SetTrigger("Run");
        }

        public bool CheckNegativeVelocity()
        {
            return _rigidbody2D.velocityY * _rigidbody2D.gravityScale <= 0;
        }
        
        public void SetAbsVelocity(float value)
        {
            _rigidbody2D.velocityY = -2 * _rigidbody2D.gravityScale;
        }

        public void LerpFlyVelocity()
        {
            Vector2 MaxVelosity = new Vector2(_rigidbody2D.velocity.x, _playerResources.MaxFlyVelosityY);

            _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity,MaxVelosity, Time.fixedDeltaTime * _playerResources.FlySmoothing);;
        }
        
        public void Die()
        {
            MovementStateMachine.SetDyingState();
        }

        private void InitializeStateMachine()
        {
            MovementStateMachine = new CharacterStateMachine(this, _playerResources, _eventManager);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsActive)
            {
                return;
            }
            OnTriggerEnter?.Invoke(other);
            Debug.Log("OnTriggerEnter2D");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsActive)
            {
                return;
            }
            OnTriggerExit?.Invoke(other);
            Debug.Log("OnTriggerExit");
        }
        
        private void RestartAnimator()
        {
            _animator.SetTrigger("Restart");
            _animator.ResetTrigger("Run");
        }

        public void Dispose()
        {
            MovementStateMachine?.Dispose();
        }
    }
}
