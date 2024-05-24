using System;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class CameraFollower : MonoBehaviour
    {
        public CameraFollowMode CameraMode = CameraFollowMode.Default;
        
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
            Vector3 nextPosition = Vector3.zero;
            switch (CameraMode)
            {
                case CameraFollowMode.Default:
                    nextPosition = Vector3.Lerp(transform.position, _targetTransform.position + _offset, Time.fixedDeltaTime * _smoothing);
                    break;
                case CameraFollowMode.FlipGravity:
                    Vector3 trueCameraPosition = new Vector3(_targetTransform.position.x + _offset.x,
                        _targetTransform.position.y - _offset.y, _targetTransform.position.z + _offset.z);
                    nextPosition = Vector3.Lerp(transform.position, trueCameraPosition , Time.fixedDeltaTime * _smoothing);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.position = nextPosition;
        }

        public void RestartCamera(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            transform.position = _targetTransform.position + _offset;
        }
    }

    public enum CameraFollowMode
    {
        Default = 0,
        FlipGravity = 1
    }
}