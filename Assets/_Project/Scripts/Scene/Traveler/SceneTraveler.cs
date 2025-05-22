using System.Collections;
using UnityEngine;
using ATBMI.Audio;

namespace ATBMI.Scene
{
    public class SceneTraveler : Traveler
    {
        // Internal fields
        [Header("Attribute")]
        [SerializeField] private bool isRoomTravel;
        [SerializeField] private LocationData locationData;
        
        private SceneAsset _currentScene;
        private const string ENTER_ROOM_STATE = "Enter_Room";
        
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

            StartCoroutine(TravelRoutine(targetScene));
        }

        private IEnumerator TravelRoutine(SceneAsset targetScene)
        {
            HandleDoorAnimation();
            DisableTravel();
            yield return new WaitForSeconds(0.15f);
            
            HandleAudioTravel(targetScene);
            SceneNavigation.Instance.SwitchScene(targetScene);
        }
        
        private void HandleDoorAnimation()
        {
            if (!isRoomTravel) return;
            iAnimatable.TrySetAnimationState(ENTER_ROOM_STATE);
        }
        
        private void HandleAudioTravel(SceneAsset targetScene)
        {
            if (_currentScene.Region != targetScene.Region) return; 
            AudioEvent.FadeOutAudioEvent();
        }
    }
}