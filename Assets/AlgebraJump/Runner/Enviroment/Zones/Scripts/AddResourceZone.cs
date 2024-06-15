using AlgebraJump.Bank;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class AddResourceZone : IResourceZone
    {
        [SerializeField] private ResourceType _resourceType = ResourceType.No;
        [SerializeField] private int _amount = 0;
        [SerializeField] private GameObject _visualRoot;
        public override void Enter(GameSessionService gameSessionService, ICharacter player)
        {
            gameSessionService.AddCollectedResource(_resourceType, _amount);
            _visualRoot.SetActive(false);
        }

        public override void Exit(GameSessionService gameSessionService, ICharacter player)
        {
            
        }
    }
}