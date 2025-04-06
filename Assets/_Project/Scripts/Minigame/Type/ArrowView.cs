using UnityEngine;
using UnityEngine.UI;

namespace ATBMI.Minigame
{
    public class ArrowView : MinigameView
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private float arrowDuration;
        [SerializeField] private int arrowCount;
        [SerializeField] private Sprite[] arrowFormSprites;
        
        private float _currentDuration;
        private int _currentArrowIndex;
        
        [Header("UI")]
        [SerializeField] private Image[] arrowImages;
        [SerializeField] private Slider arrowSlider;

        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            _currentArrowIndex = 0;
            _currentDuration = arrowDuration;
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
            _currentArrowIndex = 0;
            _currentDuration = arrowDuration;
        }

        #endregion
    }
}