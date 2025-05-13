using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour, IAnimatable
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
            // Cache animation hash
            foreach (var clip in _characterAnim.runtimeAnimatorController.animationClips)
            {
                var clipName = clip.name;
                if (!_animationHashes.ContainsKey(clipName))
                    _animationHashes[clipName] = Animator.StringToHash(clipName);
            }
        }

        // Core
        public bool TrySetAnimationState(string state)
        {
            var stateName = _characterAI.Data.AnimationTag + "_" + state;
            if (!_animationHashes.ContainsKey(stateName))
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