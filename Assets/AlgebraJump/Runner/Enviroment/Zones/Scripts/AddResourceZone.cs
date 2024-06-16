using AlgebraJump.Bank;
using UnityEngine;

namespace AlgebraJump.Runner
{
    public class AddResourceZone : IResourceZone
    {
        public override string ZoneID => _zoneID;

        [SerializeField] private string _zoneID;
        [SerializeField] private ResourceType _resourceType = ResourceType.No;
        [SerializeField] private int _amount = 0;
        [SerializeField] private GameObject _visualRoot;
        public override void Enter(GameSessionService gameSessionService, ICharacter player)
        {
            if (!enabled)
            {
                return;
            }
            
            gameSessionService.AddCollectedResource(ZoneID, _resourceType, _amount);
            _visualRoot.SetActive(false);
        }

        public override void Exit(GameSessionService gameSessionService, ICharacter player)
        {
            
        }

        public override void RestartZone()
        {
            SetActive(true);
        }

        public override void SetActive(bool active)
        {
            _visualRoot.SetActive(active);
            enabled = active;
        }
    }
}