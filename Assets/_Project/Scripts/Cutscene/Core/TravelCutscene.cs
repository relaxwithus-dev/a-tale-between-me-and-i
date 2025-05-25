using ATBMI.Gameplay.Event;
using UnityEngine;
using ATBMI.Scene;

namespace ATBMI.Cutscene
{
    public class TravelCutscene : Cutscene
    {
        #region Fields

        [Header("Attribute")] 
        [SerializeField] private bool isTravelToMenu;
        [SerializeField] private LocationData locationData;
        
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
            var currentScene = SceneNavigation.Instance.CurrentScene;
            var sceneAsset = currentScene.GetNeighbourById(locationData.location);
            
            if (!sceneAsset)
            {
                Debug.LogError("target scene not found");
                return;
            }
            
            SceneNavigation.Instance.SwitchScene(sceneAsset);
            isFinishStep = true;
        }
        
        private void TravelToMenuScene()
        {
            GameEvents.GameExitEvent();
            SceneNavigation.Instance.SwitchSceneSection(isToMenu: true);
        }
        
        #endregion
    }
}