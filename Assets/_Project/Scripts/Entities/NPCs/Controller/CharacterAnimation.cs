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
        public bool TrySetAnimationState(string state)
        {
            if (IsAnimationExists(state))
            {
                Debug.LogWarning("Animation isn't exists");
                return false;
            }
            
            _currentState = Animator.StringToHash(state);
            _characterAnim.CrossFade(_currentState, 0, 0);
            return true;
        }
        
        public float GetAnimationTime() => _characterAnim.GetCurrentAnimatorClipInfo(0).Length;
        
        private bool IsAnimationExists(string state)
        {
            return _characterAnim.runtimeAnimatorController.
                animationClips.Any(clip => clip.name == state);
        }
        
        #endregion
    }
}