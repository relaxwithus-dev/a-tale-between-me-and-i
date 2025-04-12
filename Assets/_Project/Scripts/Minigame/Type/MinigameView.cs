using UnityEngine;
using ATBMI.Gameplay.Handler;

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
        
        // Reference
        protected GameInputHandler inputHandler;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            InitOnAwake();
        }

        private void Update()
        {
            if (!isPlayMinigame) return;
            RunMinigame();
        }
        
        // Initialize
        protected virtual void InitOnAwake()
        {
            inputHandler = GameInputHandler.Instance;
        }
        
        // Core
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