using System.Collections;
using UnityEngine;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Minigame
{
    [RequireComponent(typeof(MinigameAnimation))]
    public class MinigameView : MonoBehaviour
    {
        #region Global Fields
        
        [Header("View")]
        [SerializeField] private bool isPlayMinigame;
        [SerializeField] protected float closeMinigameDelay = 0.15f;
        [SerializeField] private MinigameManager minigameManager;
        
        protected int playingCount;
        protected const float MAX_SLIDER_VALUE = 1f;
        protected const float MIN_SLIDER_VALUE = 0f;
        
        // Reference
        protected GameInputHandler inputHandler;
        private MinigameAnimation _minigameAnimation;
        
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
            _minigameAnimation = GetComponent<MinigameAnimation>();
        }
        
        // Core
        public virtual void EnterMinigame()
        {
            _minigameAnimation.OpenMinigame(() => isPlayMinigame = true);
        }
        protected virtual void RunMinigame() { }
        protected void ExitMinigame()
        {
            isPlayMinigame = false;
            StartCoroutine(ExitMinigameRoutine());
        }
        
        private IEnumerator ExitMinigameRoutine()
        {
            yield return new WaitForSeconds(closeMinigameDelay);
            _minigameAnimation.CloseMinigame(minigameManager.ExitMinigame);
        }
        
        #endregion
    }
}