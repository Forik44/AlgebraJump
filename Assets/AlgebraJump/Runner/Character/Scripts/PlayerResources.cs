using UnityEngine;
using UnityEngine.Serialization;

namespace AlgebraJump.Runner
{
    [CreateAssetMenu(fileName = "PlayerResources", menuName = "PlayerResources", order = 51)]
    public class PlayerResources : ScriptableObject
    {
        [FormerlySerializedAs("Speed")] 
        [SerializeField] private float _speed = 1.0f;
        
        [FormerlySerializedAs("Player smoothing")] 
        [SerializeField] private float _smoothing = 1.0f;
        
        [FormerlySerializedAs("Fly smoothing")]
        [SerializeField] private float _flySmoothing = 1.0f;

        [FormerlySerializedAs("Max Fly Velosity Y")]
        [SerializeField] private float _maxFlyVelosityY = 10;
        
        [FormerlySerializedAs("Fly Sprite")]
        [SerializeField] private Sprite _flySprite;

        public float Speed => _speed;
        public float Smoothing => _smoothing;
        public float FlySmoothing => _flySmoothing;
        public float MaxFlyVelosityY => _maxFlyVelosityY;
        public Sprite FlySprite => _flySprite;

    }
}