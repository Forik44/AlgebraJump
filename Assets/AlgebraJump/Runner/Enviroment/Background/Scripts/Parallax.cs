using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private float _parallaxEffect = 1f;
    
        private CameraFollower _camera;
        private float _lenght;
        private float _startPosition;

        public void Initialize(CameraFollower camera)
        {
            _camera = camera;
            _startPosition = transform.position.x;
            _lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    
        void Update()
        {
            if (!_camera)
            {
                return;
            }

            var cameraPositionX = _camera.transform.position.x;
            float bound = (cameraPositionX * (1 - _parallaxEffect));
            float distance = (cameraPositionX * _parallaxEffect);

            transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

            if (bound > _startPosition + _lenght/2)
            {
                _startPosition += _lenght;
            }
            else if (bound < _startPosition - _lenght/2)
            {
                _startPosition -= _lenght;
            }
        }
    }
}

