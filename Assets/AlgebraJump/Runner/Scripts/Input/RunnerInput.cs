using UnityEngine;

namespace AlgebraJump.Runner
{
    public abstract class RunnerInput : MonoBehaviour
    {
        protected Character Player { get; private set; }
        
        public void Bind(Character player)
        {
            Player = player;
        }
    }
}