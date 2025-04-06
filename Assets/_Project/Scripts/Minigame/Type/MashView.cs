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
        
        // Unity Callbacks
        private void Start()
        {
            
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
            
        }

        #endregion
    }
}