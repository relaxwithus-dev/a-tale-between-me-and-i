using UnityEngine;
using Cinemachine;

namespace ATBMI.Gameplay.Controller
{
    public class CameraSwitcher : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Camera")]
        [SerializeField] private bool isCameraSwitching;
        [SerializeField] private CinemachineBrain cameraBrain;
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        
        private CinemachineVirtualCamera _currentCamera;

        private readonly int MaxVirtualCamera = 2;
        private readonly int ActivePriority = 10;
        private readonly int InactivePriority = 0;
        
        public float SwitchDuration { get; private set; }
        public CinemachineVirtualCamera[] VirtualCameras => virtualCameras;
        public static CameraSwitcher Instance { get; private set; }

        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        
        private void Start()
        {
            // Validate
            if (virtualCameras.Length < MaxVirtualCamera)
            {
                Debug.LogWarning("virtual camera isn't enough!");
                return;
            }
            
            isCameraSwitching = false;
            _currentCamera = virtualCameras[0];
            SwitchDuration = cameraBrain.m_DefaultBlend.BlendTime;
        }
        
        // Core
        public void SwitchToMainCamera()
        {
            if (isCameraSwitching) return;
            
            var virtualCam = virtualCameras[0];
            SwitchCamera(virtualCam);
        }
        
        public void SwitchToSubCamera(Transform point)
        {
            if (isCameraSwitching) return;
            
            var virtualCam = virtualCameras[1];
            virtualCam.transform.position = new Vector3(point.position.x, 
                point.position.y, virtualCam.transform.position.z);
            SwitchCamera(virtualCam);
        }
        
        private void SwitchCamera(CinemachineVirtualCamera virtualCam)
        {
            isCameraSwitching = true;
            
            _currentCamera = virtualCam;
            _currentCamera.enabled = true;
            _currentCamera.Priority = ActivePriority;
            
            foreach (var vCam in virtualCameras)
            {
                if (vCam != _currentCamera)
                {
                    vCam.Priority = InactivePriority;
                    vCam.enabled = false;
                }
            }
            
            isCameraSwitching = false;
        }
        
        #endregion
    }
}