using UnityEngine;
using ATBMI.Scene;

namespace ATBMI.Cutscene
{
    public class TravelCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")]
        [SerializeField] private LocationData locationData;
        private bool _isFinished;
        
        #endregion

        #region Methods
        
        public override void Execute()
        {
            var currentScene = SceneNavigation.Instance.CurrentScene;
            var sceneAsset = currentScene.GetNeighbourById(locationData.location);
            
            if (!sceneAsset)
            {
                Debug.LogError("target scene not found");
                return;
            }
            
            SceneNavigation.Instance.SwitchScene(sceneAsset);
            _isFinished = true;
        }
        
        public override bool IsFinished() => _isFinished;
        
        #endregion
    }
}