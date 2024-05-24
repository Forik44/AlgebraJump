using System;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Lukomor.AlgebraJump.Runner
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        public StateMachine MovementSM;
        public RunState Running;
        public JumpState Jumping;
        public FlyState Flying;

        public UnityEvent OnPositionChanged;
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;
        
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _checkGroundRadius = 0.3f;
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private Transform _groundCheckerPivot;
        [SerializeField] private LayerMask _groundMask;

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

            PlayerState.TryStartFly = false;
            MovementSM.ChangeState(Running);
        }

        public void SetDoubleJump(bool doubleJump)
        {
            _hasDoubleJump = doubleJump;
        }
        
        public void FlipGravity()
        {
            _rigidbody2D.velocityY = 0;
            _rigidbody2D.gravityScale *= -1;

            if (_rigidbody2D.gravityScale == 1)
            {
                _cameraFollower.SetCameraFollowMode(CameraFollowMode.Default);
            }
            else if (_rigidbody2D.gravityScale == -1)
            {
                _cameraFollower.SetCameraFollowMode(CameraFollowMode.FlipGravity);
            }
        }
        public void FlipFlyState()
        {
            if (MovementSM.CurrentState != Flying)
            {
                StartFly();
            }
            else
            {
                StopFly();
            }
            //TODO: тернарный оператор
        }
        public void StartFly()
        {
            _cameraFollower.SetCameraFollowMode(CameraFollowMode.FlyState);
            PlayerState.TryStartFly = true;
        }
        
        public void StopFly()
        {
            _cameraFollower.SetCameraFollowMode(CameraFollowMode.Default);
            PlayerState.TryStopFly = true;
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
        private void Start()
        {
            InitializeStateMachine();
            Restart();
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

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void InitializeStateMachine()
        {
            MovementSM = new StateMachine();

            Running = new RunState(this, MovementSM, _playerResources);
            Jumping = new JumpState(this, MovementSM, _playerResources);
            Flying = new FlyState(this, MovementSM, _playerResources);

            MovementSM.Initialize(Running);
        }

        private void Update()
        {
            MovementSM.CurrentState.HandleInput();
            MovementSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            MovementSM.CurrentState.PhysicsUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnter?.Invoke(other);
            Debug.Log("OnTriggerEnter2D");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExit?.Invoke(other);
            Debug.Log("OnTriggerExit");
        }
    }
}
