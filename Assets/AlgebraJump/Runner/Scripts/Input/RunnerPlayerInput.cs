using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class RunnerPlayerInput : RunnerInput
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.Jump();
            }
        }
    }
}
