using System;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public interface ICharacter
    {
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;
        public event Action<float> OnPositionChanged;

        public void StopMoving();
        public void StartMoving();
        public void FlipGravity();
        public void FlipFlyState();
        public void SetDoubleJump(bool b);
        public void Die();
    }
}