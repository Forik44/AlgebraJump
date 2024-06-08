using AlgebraJump.Bank;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class AddResourceZone : IZone
    {
        [SerializeField] private ResourceType _resourceType = ResourceType.No;
        [SerializeField] private int _amount = 0;
        public override void Enter(GameSessionService gameSessionService, ICharacter player)
        {
            gameSessionService.AddCollectedResource(_resourceType, _amount);
        }

        public override void Exit(GameSessionService gameSessionService, ICharacter player)
        {
            
        }
    }
}