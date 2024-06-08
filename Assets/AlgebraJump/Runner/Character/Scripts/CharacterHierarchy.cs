using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AlgebraJump.Runner
{
    public class CharacterHierarchy : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;
        public event Action OnRestart;

        public Transform GroundCheckerPivot;
        public LayerMask GroundMask;
        public Animator Animator;
        public Transform VisualRoot;
        public Rigidbody2D Rigidbody;

        public void Restart()
        {
            OnRestart.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled)
            {
                return;
            }
            OnTriggerEnter?.Invoke(other);
            Debug.Log("OnTriggerEnter2D");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!enabled)
            {
                return;
            }
            OnTriggerExit?.Invoke(other);
            Debug.Log("OnTriggerExit");
        }
    }
}