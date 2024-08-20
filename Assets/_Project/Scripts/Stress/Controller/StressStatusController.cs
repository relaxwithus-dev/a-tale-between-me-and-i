using UnityEngine;
using ATBMI.Data;
using ATBMI.Player;
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
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
            // Status
            ProductivityStatus = new Productivity(productivityData, _playerController);
            DepressionStatus = new Depression(depressionData, _playerController, _stressAnimator);
        }
        
        private void OnEnable()
        {
            PlayerEvents.OnStressActive += HandleActiveStatus;
            PlayerEvents.OnStressInactive += HandleInactiveStatus;
        }

        private void OnDisable()
        {
            PlayerEvents.OnStressActive -= HandleActiveStatus;
            PlayerEvents.OnStressInactive -= HandleInactiveStatus;
        }

        #endregion
        
        #region Methods

        private void HandleActiveStatus()
        {
            if (_currentStatus == null)
            {
                var isProductivity = GetRandomValue();
                _currentStatus = isProductivity ? ProductivityStatus :  DepressionStatus;
            }
            _currentStatus.PerformStatus();
        }

        private void HandleInactiveStatus()
        {
            _currentStatus.AvoidStatus();
            _currentStatus = null;
        }

        private bool GetRandomValue() => Random.value > 0.5f;

        #endregion

    }
}