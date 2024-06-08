using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlgebraJump.Runner
{
    [CreateAssetMenu(fileName = "SpawnerPrefabs", menuName = "SpawnerPrefabs", order = 51)]
    public class SpawnerFactory : ScriptableObject
    {
        [SerializeField] private CharacterHierarchy _player;
        [SerializeField] private CameraFollower _camera;

        public CharacterHierarchy SpawnPlayer()
        {
            return Instantiate(_player);
        }

        public CameraFollower SpawnCamera()
        {
            return Instantiate(_camera);
        }
    }
}
