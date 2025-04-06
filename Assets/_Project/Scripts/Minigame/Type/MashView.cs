using UnityEngine;

namespace ATBMI.Minigame
{
    public class MashView : MinigameView
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private float maxValue;
        [SerializeField] private float clickPower;
        [SerializeField] private float decreasePercentage;

        private float _currentValue;

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