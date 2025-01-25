using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Event;

namespace ATBMI.Stress
{
    public class StressStatusController : MonoBehaviour
    {
        #region Fields & Property

        [Header("Data")]
        [SerializeField] private StressData productivityData;
        [SerializeField] private StressData depressionData;

        private StressStatus _currentStatus;
        public Productivity ProductivityStatus { get; private set; }
        public Depression DepressionStatus { get; private set; }

        // Reference
        private Animator _stressAnimator;
        private PlayerController _playerController;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // Component
            _stressAnimator = GetComponentInChildren<Animator>();
            _playerController = GetComponentInParent<PlayerController>();
        
            // Status
            ProductivityStatus = new Productivity(productivityData, _playerController);
            DepressionStatus = new Depression(depressionData, _playerController, _stressAnimator);
        }
        
        private void OnEnable()
        {
            PlayerEvents.OnStressActive += ActivateStatus;
            PlayerEvents.OnStressInactive += InactiveStatus;
        }

        private void OnDisable()
        {
            PlayerEvents.OnStressActive -= ActivateStatus;
            PlayerEvents.OnStressInactive -= InactiveStatus;
        }

        #endregion
        
        #region Methods

        private void ActivateStatus()
        {
            _currentStatus = Random.value > 0.5f ? ProductivityStatus :  DepressionStatus;
            _currentStatus.PerformStatus();
        }

        private void InactiveStatus()
        {
            _currentStatus.ResetStatus();
            _currentStatus = null;
        }
        
        #endregion

    }
}