using System;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Lukomor.AlgebraJump.Runner
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour, IPlayer
    {
        public UnityEvent OnPositionChanged;
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;
        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _smoothing = 1f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _checkGroundRadius = 0.3f;
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private Transform _groundCheckerPivot;
        [SerializeField] private LayerMask _groundMask;
        
        private bool _isGrounded;
        private Rigidbody2D _rigidbody2D;
        private Vector3 _initialPosition;
        private CameraFollower _cameraFollower;
        private bool _hasDoubleJump;

        public bool IsActive
        {
            get => enabled;
            set => enabled = value;
        }
        
        public void Jump()
        {
            if (!_hasDoubleJump && !_isGrounded || !IsActive)
            {
               return;
            }

            SetDoubleJump(false);
            
            _rigidbody2D.velocityY = (float)Math.Sqrt(_jumpHeight * -2 * _gravity) * _rigidbody2D.gravityScale;
        }

        public void Initialize(Transform transform, CameraFollower cameraFollower)
        {
            _initialPosition = transform.position;
            _cameraFollower = cameraFollower;
            _cameraFollower.RestartCamera(transform);
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
        }

        public void SetDoubleJump(bool doubleJump)
        {
            _hasDoubleJump = doubleJump;
        }
        
        public void FlipGravity()
        {
            _rigidbody2D.velocityY = 0;
            _rigidbody2D.gravityScale *= -1;
        }
        private void Start()
        {
            Restart();
        }

        private bool IsOnTheGround()
        {
            Vector2 groundCheckerPosition = _groundCheckerPivot.position;
            
            RaycastHit2D hit = Physics2D.Raycast(groundCheckerPosition, -Vector2.up * _rigidbody2D.gravityScale, _checkGroundRadius, _groundMask);
            
            return hit.collider != null;
        }
        
        private void Move(Vector3 direction)
        {
            if (!IsActive)
            {
                return;
            }
            
            var nextPosition = Vector3.Lerp(transform.position, transform.position + direction * _speed, Time.deltaTime * _smoothing);

            transform.position = nextPosition;
            OnPositionChanged?.Invoke();
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _isGrounded = IsOnTheGround();
            
            if (_isGrounded && _rigidbody2D.velocityY * _rigidbody2D.gravityScale <= 0)
            { 
                _rigidbody2D.velocityY = -2 * _rigidbody2D.gravityScale;
            }
            
            Move(Vector2.right);
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
