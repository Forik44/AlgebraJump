using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public abstract class RunnerInput : MonoBehaviour
    {
        protected PlayerView Player { get; private set; }
        
        public void Bind(PlayerView player)
        {
            Player = player;
        }
    }
}