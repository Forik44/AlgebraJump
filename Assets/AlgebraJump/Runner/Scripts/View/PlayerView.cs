using System;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.AlgebraJump.Runner
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        public UnityEvent OnPositionChanged;
        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _smoothing = 1f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _checkGroundRadius = 0.3f;
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private Transform _groundCheckerPivot;
        [SerializeField] private LayerMask _groundMask;
        
        private bool _isGrounded;
        private Rigidbody2D _rigidbody2D;

        public bool IsActive
        {
            get => enabled;
            set => enabled = value;
        }

        public void Jump()
        {
            if (!IsActive || !_isGrounded)
            {
               return;
            }
            
            _rigidbody2D.velocityY = (float)Math.Sqrt(_jumpHeight * -2 * _gravity);
        }

        bool IsOnTheGround()
        {
            Vector2 groundCheckerPosition = _groundCheckerPivot.position;
            
            RaycastHit2D hit = Physics2D.Raycast(groundCheckerPosition, -Vector2.up, _checkGroundRadius, _groundMask);
            
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
            
            if (_isGrounded && _rigidbody2D.velocityY <= 0)
            { 
                _rigidbody2D.velocityY = -2;
            }
            
            Move(Vector2.right);
        }
    }
}
