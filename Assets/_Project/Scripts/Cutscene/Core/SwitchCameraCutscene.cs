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
        
        private bool _isFinished;
        private CameraSwitcher _cameraSwitcher;
        
        #endregion

        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            _isFinished = false;
            _cameraSwitcher = CameraSwitcher.Instance;
        }
        
        public override void Execute()
        {
            StartCoroutine(SwitchCameraRoutine());
        }
        
        public override bool IsFinished() => _isFinished;
        
        private IEnumerator SwitchCameraRoutine()
        {
            var switchDuration = _cameraSwitcher.SwitchDuration;
            _cameraSwitcher.SwitchToSubCamera(cameraPoint);
            yield return new WaitForSeconds(holdDuration + switchDuration);
            
            _cameraSwitcher.SwitchToMainCamera();
            yield return new WaitForSeconds(switchDuration);
            _isFinished = true;
        }
        
        #endregion
    }
}