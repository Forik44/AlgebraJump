using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using AlgebraJump.Runner;
using AlgebraJump;

namespace AlgebraJump.Levels
{
    public class LevelsService
    {
        public string CurrentLevelID { get; private set; }
        
        private List<IResourceZone> _resourceZones;
        
        private readonly LevelsData _levelsData;
        private readonly ScenesService _scenesService;
        private readonly IGameStateSaver _gameStateSaver;

        public LevelsService(LevelsData levelsData, ScenesService scenesService, IGameStateSaver gameStateSaver)
        {
            _levelsData = levelsData;
            _scenesService = scenesService;
            _gameStateSaver = gameStateSaver;

            _scenesService.StartLoadGameplayScene += LoadLevel;
        }

        public void SaveCollectableResourceZone(string levelID, string zoneID)
        {
            _levelsData.Levels[levelID].CollectedResourceZonesInLevels.Add(zoneID); 
            _gameStateSaver.SaveGameState();
        }
        
        public void SaveCollectableResourceZoneList(string levelID, List<string> zoneIDs)
        {
            foreach (var zoneID in zoneIDs)
            {
                SaveCollectableResourceZone(levelID, zoneID);
            }
        }

        public bool ContainsCollectedZoneID(string levelID, string zoneID)
        {
            return _levelsData.Levels[levelID].CollectedResourceZonesInLevels.Contains(zoneID);
        }

        public void RestartLevel(Unit unit)
        {
            RestartResourceZone();
        }
        
        public void SetupResourceZones(IResourceZone[] resourceZones)
        {
            _resourceZones = resourceZones.ToList();
        }
        
        public void RestartResourceZone()
        {
            foreach (var resourceZone in _resourceZones)
            {
                resourceZone.RestartZone();
            }
            
            foreach (var resourceZone in _resourceZones)
            {
                bool containsCollectedZoneID = ContainsCollectedZoneID(CurrentLevelID, resourceZone.ZoneID);
                
                if (containsCollectedZoneID) 
                {
                    resourceZone.SetActive(false);
                }
            }
        }
        
        private void LoadLevel(string levelId)
        {
            _scenesService.LoadLevel(_levelsData.Levels[levelId].SceneName);
            CurrentLevelID = levelId;
        }
    }
}