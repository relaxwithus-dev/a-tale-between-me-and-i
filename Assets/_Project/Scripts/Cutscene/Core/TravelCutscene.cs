using UnityEngine;
using ATBMI.Scene;
using ATBMI.Audio;
using ATBMI.Gameplay.Event;

namespace ATBMI.Cutscene
{
    public class TravelCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")] 
        [SerializeField] private bool isTravelToMenu;
        [SerializeField] private LocationData locationData;
        
        private SceneAsset _currentScene;
        
        #endregion
        
        #region Methods
        
        public override void Execute()
        {
            if (isTravelToMenu)
                TravelToMenuScene();
            else
                TravelToNextScene();
            
            isFinishStep = true;
        }
        
        private void TravelToNextScene()
        {
            _currentScene = SceneNavigation.Instance.CurrentScene;
            var sceneAsset = _currentScene.GetNeighbourById(locationData.location);
            
            if (!sceneAsset)
            {
                Debug.LogError("target scene not found");
                return;
            }
            
            HandleAudioTravel(sceneAsset);
            SceneNavigation.Instance.SwitchScene(sceneAsset);
            isFinishStep = true;
        }
        
        private void TravelToMenuScene()
        {
            GameEvents.GameExitEvent();
            SceneNavigation.Instance.SwitchSceneSection(isToMenu: true);
        }
        
        private void HandleAudioTravel(SceneAsset targetScene)
        {
            if (_currentScene.Region == targetScene.Region) return; 
            AudioEvent.FadeOutAudioEvent();
        }
        
        #endregion
    }
}