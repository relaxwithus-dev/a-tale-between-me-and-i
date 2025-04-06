using UnityEngine;

namespace ATBMI.Minigame
{
    public class MinigameView : MonoBehaviour
    {
        #region Global Fields
        
        [Header("View")]
        [SerializeField] private GameObject minigamePanelUI;
        [SerializeField] private MinigameManager minigameManager;
        [SerializeField] private bool isPlayMinigame;
        
        protected const float MAX_SLIDER_VALUE = 1f;
        protected const float MIN_SLIDER_VALUE = 0f;
        
        #endregion

        #region Methods
        
        private void Start()
        {
            InitOnStart();
        }
        
        private void Update()
        {
            if (!isPlayMinigame) return;
            RunMinigame();
        }
        
        // Core
        protected virtual void InitOnStart() { }
        
        public virtual void EnterMinigame()
        {
            isPlayMinigame = true;
        }
        
        public virtual void ExitMinigame()
        {
            isPlayMinigame = false;
            minigameManager.ExitMinigame();
        }
        
        protected virtual void RunMinigame() { }
        
        #endregion
    }
}