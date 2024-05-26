using System;
using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Entities.Player;
using UnityEngine;

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

        #endregion

    }
}