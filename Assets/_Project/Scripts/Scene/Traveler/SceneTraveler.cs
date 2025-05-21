using UnityEngine;
using ATBMI.Audio;

namespace ATBMI.Scene
{
    public class SceneTraveler : Traveler
    {
        // Internal fields
        [SerializeField] private LocationData locationData;
        private SceneAsset _currentScene;
        
        // Core
        protected override void TravelToTarget()
        {
            _currentScene = SceneNavigation.Instance.CurrentScene;
            var targetScene = _currentScene.GetNeighbourById(locationData.location);
            
            if (!targetScene)
            {
                Debug.LogError("target scene not found");
                return;
            }
            
            DisableTravel();
            HandleAudioTravel(targetScene);
            SceneNavigation.Instance.SwitchScene(targetScene);
        }
        
        private void HandleAudioTravel(SceneAsset targetScene)
        {
            if (_currentScene.Region != targetScene.Region) return; 
            AudioEvent.FadeOutAudioEvent();
        }
    }
}