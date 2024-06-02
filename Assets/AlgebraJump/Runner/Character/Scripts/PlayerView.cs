using System;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace AlgebraJump.Runner
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        public UnityEvent OnPositionChanged;
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;
        
        public PlayerMovementStateMachine MovementStateMachine { get; private set; }
        public bool TryStartFly = false;
        public bool TryStopFly = false;
        public bool TryJump = false;
        
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _checkGroundRadius = 0.5f;
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private Transform _groundCheckerPivot;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visualRoot;
        
        private Rigidbody2D _rigidbody2D;
        private Vector3 _initialPosition;
        private CameraFollower _cameraFollower;
        private PlayerResources _playerResources;
        private bool _hasDoubleJump = false;

        public bool IsActive
        {
            get => enabled;
            set => enabled = value;
        }
        
        public void Jump()
        {
            if (!IsActive)
            {
               return;
            }

            _rigidbody2D.velocityY = (float)Math.Sqrt(_jumpHeight * -2 * _gravity) * _rigidbody2D.gravityScale;
        }

        public void Initialize(Transform transform, CameraFollower cameraFollower, PlayerResources playerResources)
        {
            _initialPosition = transform.position;
            _cameraFollower = cameraFollower;
            _cameraFollower.RestartCamera(transform);
            _playerResources = playerResources;
        }

        public void StopMoving()
        {
            enabled = false;
            _rigidbody2D.Sleep();
        }
        
        public void StartMoving()
        {
            enabled = true;
            _rigidbody2D.WakeUp();
        }

        public void Restart()
        {
            transform.position = _initialPosition;
            _cameraFollower.RestartCamera(transform);
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
            transform.position = nextPosition;
            OnPositionChanged?.Invoke();
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
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            InitializeStateMachine();
            Restart();
        }
        
        private void InitializeStateMachine()
        {
            MovementStateMachine = new PlayerMovementStateMachine(this,_playerResources);
        }

        private void Update()
        {
            MovementStateMachine.CurrentState.HandleInput();
            MovementStateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            MovementStateMachine.CurrentState.PhysicsUpdate();
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
    }
}
