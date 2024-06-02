using System;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class CameraFollower : MonoBehaviour
    {
        private CameraFollowMode _cameraMode = CameraFollowMode.Default;
        
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothing = 1f;
        
        private Transform _targetTransform;
        
        public void RestartCamera(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            transform.position = _targetTransform.position + _offset;
            SetCameraFollowMode(CameraFollowMode.Default);
        }
        public void SetCameraFollowMode(CameraFollowMode followMode)
        {
            _cameraMode = followMode;
        }
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
            switch (_cameraMode)
            {
                case CameraFollowMode.Default:
                    nextPosition = Vector3.Lerp(transform.position, _targetTransform.position + _offset, Time.fixedDeltaTime * _smoothing);
                    break;
                
                case CameraFollowMode.FlipGravity:
                    Vector3 trueCameraPosition = new Vector3(_targetTransform.position.x + _offset.x,
                        _targetTransform.position.y - _offset.y, _targetTransform.position.z + _offset.z);
                    nextPosition = Vector3.Lerp(transform.position, trueCameraPosition , Time.fixedDeltaTime * _smoothing);
                    break;
                
                case CameraFollowMode.FlyState:
                    Vector3 trueCameraFlyPosition = new Vector3(_targetTransform.position.x + _offset.x,
                        _targetTransform.position.y, _targetTransform.position.z + _offset.z);
                    nextPosition = Vector3.Lerp(transform.position, trueCameraFlyPosition , Time.fixedDeltaTime * _smoothing);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.position = nextPosition;
        }
    }

    public enum CameraFollowMode
    {
        Default = 0,
        FlipGravity = 1,
        FlyState = 2
    }
}