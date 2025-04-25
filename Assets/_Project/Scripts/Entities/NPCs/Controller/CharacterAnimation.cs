using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour
    {
        #region Fields & Properties
        
        private int _currentState;
        private readonly Dictionary<string, int> _animationHashes = new();
        
        // Reference
        private Animator _characterAnim;
        private CharacterAI _characterAI;
        
        #endregion

        #region  Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _characterAnim = GetComponent<Animator>();
            _characterAI = GetComponentInParent<CharacterAI>();
        }

        private void Start()
        {
            foreach (var clip in _characterAnim.runtimeAnimatorController.animationClips)
            {
                CacheAnimationHash(clip.name);
            }
        }

        // Core
        public bool TrySetAnimationState(string state)
        {
            var stateName = _characterAI.Data.AnimationTagName + "_" + state;
            if (_animationHashes.ContainsKey(stateName))
            {
                Debug.LogWarning("Animation isn't exists");
                return false;
            }
            
            _currentState = GetCachedHash(stateName);
            _characterAnim.CrossFade(_currentState, 0, 0);
            return true;
        }
        
        public float GetAnimationTime() => _characterAnim.GetCurrentAnimatorClipInfo(0).Length;
        
        // Helpers
        private void CacheAnimationHash(string animName)
        {
            if (!_animationHashes.ContainsKey(animName))
                _animationHashes[animName] = Animator.StringToHash(animName);
        }

        private int GetCachedHash(string animName)
        {
            if (_animationHashes.TryGetValue(animName, out var hash))
                return hash;
            
            hash = Animator.StringToHash(animName);
            _animationHashes[animName] = hash;
            return hash;
        }
        
        #endregion
    }
}