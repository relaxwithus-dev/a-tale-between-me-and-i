using System;
using UnityEngine;

namespace ATBMI.Minigame
{
    public class MinigameView : MonoBehaviour
    {
        #region Global Fields
        
        [Header("View")]
        [SerializeField] private MinigameManager minigameManager;
        [SerializeField] private bool isPlayMinigame;

        protected int playingCount;
        
        protected const float MAX_SLIDER_VALUE = 1f;
        protected const float MIN_SLIDER_VALUE = 0f;
        
        #endregion

        #region Methods

        private void Awake()
        {
            InitOnAwake();
        }

        private void Update()
        {
            if (!isPlayMinigame) return;
            RunMinigame();
        }
        
        // Core

        protected virtual void InitOnAwake(){ }
        public virtual void EnterMinigame()
        {
            isPlayMinigame = true;
        }
        protected virtual void RunMinigame() { }
        protected virtual void ExitMinigame()
        {
            isPlayMinigame = false;
            minigameManager.ExitMinigame();
        }
        
        
        #endregion
    }
}