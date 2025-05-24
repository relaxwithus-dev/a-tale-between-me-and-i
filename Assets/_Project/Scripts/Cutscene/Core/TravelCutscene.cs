using UnityEngine;
using ATBMI.Scene;
using ATBMI.Gameplay.Event;

namespace ATBMI.Cutscene
{
    public class TravelCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")]
        [SerializeField] private bool isToMenu;
        [SerializeField] private LocationData locationData;
        
        #endregion

        #region Methods
        
        public override void Execute()
        {
            if (isToMenu)
                TravelToMenu();
            else
                TravelToLocation();
            
            isFinishStep = true;
        }

        private void TravelToMenu()
        {
            GameEvents.GameExitEvent();
            SceneNavigation.Instance.SwitchSceneSection(isToMenu: true);
        }

        private void TravelToLocation()
        {
            var currentScene = SceneNavigation.Instance.CurrentScene;
            var sceneAsset = currentScene.GetNeighbourById(locationData.location);
            
            if (!sceneAsset)
            {
                Debug.LogError("target scene not found");
                return;
            }
            
            SceneNavigation.Instance.SwitchScene(sceneAsset);
        }
        
        #endregion
    }
}