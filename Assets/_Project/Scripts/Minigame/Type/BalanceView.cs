using UnityEngine;

namespace ATBMI.Minigame
{
    public class BalanceView : MinigameView
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private float duration;
        [SerializeField] private float[] speedRange;
        [SerializeField] private float barWidth;
        
        private float _currentDuration;

        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            _currentDuration = duration;
        }
        
        private void Update()
        {
            
        }
        
        // Core
        public override void EnterMinigame() 
        {
            
        }

        public override void ExitMinigame()
        {
            _currentDuration = duration;
        }

        #endregion
    }
}