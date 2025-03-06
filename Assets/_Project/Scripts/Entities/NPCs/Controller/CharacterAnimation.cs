using System.Linq;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour
    {
        #region Fields & Properties
        
        private int _currentState;
        private Animator _characterAnim;
        
        #endregion

        #region  Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _characterAnim = GetComponent<Animator>();
        }
        
        // Core
        public void SetupAnimationState(string state)
        {
            if (!IsAnimationExists(state)) return;
            
            _currentState = Animator.StringToHash(state);
            _characterAnim.CrossFade(_currentState, 0, 0);
        }
        
        private bool IsAnimationExists(string state)
        {
            return _characterAnim.runtimeAnimatorController.
                animationClips.Any(clip => clip.name == state);
        }
        
        #endregion
    }
}