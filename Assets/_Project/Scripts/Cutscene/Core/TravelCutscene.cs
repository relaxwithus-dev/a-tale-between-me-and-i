using UnityEngine;
using ATBMI.Scene;

namespace ATBMI.Cutscene
{
    public class TravelCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")]
        [SerializeField] private LocationData locationData;
        
        #endregion

        #region Methods
        
        public override void Execute()
        {
            // var currentScene = SceneNavigation.Instance.CurrentScene;
            // var sceneAsset = currentScene.GetNeighbourById(locationData.location);
            //
            // if (!sceneAsset)
            // {
            //     Debug.LogError("target scene not found");
            //     return;
            // }
            //
            // SceneNavigation.Instance.SwitchScene(sceneAsset);
            SceneNavigation.Instance.SwitchSceneSection(isToMenu: true);
            isFinishStep = true;
        }
        
        #endregion
    }
}