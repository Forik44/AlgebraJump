using System;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothing = 1f;
        
        private Transform _targetTransform;
        private void FixedUpdate()
        {
            if (!_targetTransform)
            {
                return;
            }
            
            Move();
        }

        private void Move()
        {
            var nextPosition = Vector3.Lerp(transform.position, _targetTransform.position + _offset, Time.fixedDeltaTime * _smoothing);

            transform.position = nextPosition;
        }

        public void BindTargetTransform(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            transform.position = _targetTransform.position + _offset;
        }
    }
}