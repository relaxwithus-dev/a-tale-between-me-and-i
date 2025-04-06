using UnityEngine;

namespace ATBMI.Minigame
{
    public class BalanceView : MinigameView
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private float balanceDuration;
        [SerializeField] private float[] speedRange;
        [SerializeField] private float barWidth;
        
        #endregion
        
        #region Methods
        
        // Core
        public override void EnterMinigame() 
        {
            base.EnterMinigame();
        }

        protected override void RunMinigame()
        {
            base.RunMinigame();
        }
        
        public override void ExitMinigame()
        {
            base.ExitMinigame();
        }

        #endregion
    }
}