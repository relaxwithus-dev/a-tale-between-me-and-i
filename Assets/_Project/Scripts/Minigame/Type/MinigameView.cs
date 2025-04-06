using UnityEngine;

namespace ATBMI.Minigame
{
    public class MinigameView : MonoBehaviour
    {
        // Global fields
        [Header("View")]
        [SerializeField] private MinigameManager minigameManager;
        
        protected const float MAX_SLIDER_VALUE = 1f;
        protected const float MIN_SLIDER_VALUE = 0f;
        
        // Methods
        public virtual void EnterMinigame() { }
        public virtual void ExitMinigame() { }
    }
}