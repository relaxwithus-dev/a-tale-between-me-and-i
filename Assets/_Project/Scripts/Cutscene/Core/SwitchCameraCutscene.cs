using System.Collections;
using UnityEngine;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Cutscene
{
    public class SwitchCameraCutscene : Cutscene
    {
        #region Fields

        [Header("Attribute")]
        [SerializeField] private float holdDuration;
        [SerializeField] private Transform cameraPoint;
        
        private CameraSwitcher _cameraSwitcher;
        
        #endregion

        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            isFinishStep = false;
            _cameraSwitcher = CameraSwitcher.Instance;
        }
        
        public override void Execute()
        {
            StartCoroutine(SwitchCameraRoutine());
        }
        
        private IEnumerator SwitchCameraRoutine()
        {
            var switchDuration = _cameraSwitcher.SwitchDuration;
            _cameraSwitcher.SwitchToSubCamera(cameraPoint);
            yield return new WaitForSeconds(holdDuration + switchDuration);
            
            _cameraSwitcher.SwitchToMainCamera();
            yield return new WaitForSeconds(switchDuration);
            isFinishStep = true;
        }
        
        #endregion
    }
}