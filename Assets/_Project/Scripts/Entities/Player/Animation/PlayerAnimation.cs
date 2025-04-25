using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        #region Fields & Properties
        
        private int _currentState;
        private bool _isInteractiveAnimation;
        private readonly Dictionary<string, int> _animationHashes = new();
        
        // Reference
        private PlayerController _playerController;
        private Animator _playerAnimator;

        #endregion

        #region Unity Methods
        
        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            AnimationStateHandler();
        }

        #endregion

        #region Methods
        
        // Initialize
        public void InitAnimationHash()
        {
            _animationHashes.Clear();
            foreach (var clip in _playerAnimator.runtimeAnimatorController.animationClips)
            {
                CacheAnimationHash(clip.name);
            }
        }
        
        // Core
        private void AnimationStateHandler()
        {
            if (_isInteractiveAnimation) return;
            var state = GetState();

            if (state == _currentState) return;
            _playerAnimator.CrossFade(state, 0, 0);
            _currentState = state;
        }

        public IEnumerator TrySetAnimationState(string state)
        {
            var stateName = _playerController.Data.PlayerAnimationTag + "_" + state;
            if (!_animationHashes.ContainsKey(stateName))
            {
                Debug.LogWarning($"animation {stateName} isn't exist");
                yield break;
            }

            _isInteractiveAnimation = true;
            _currentState = GetCachedHash(stateName);
            _playerAnimator.CrossFade(_currentState, 0, 0);
            
            yield return new WaitForSeconds(_playerAnimator.GetCurrentAnimatorClipInfo(0).Length);
            _isInteractiveAnimation = false;
        }
        
        private int GetState()
        {
            var animTag = _playerController.Data.PlayerAnimationTag;
            return _playerController.PlayerState switch
            {
                PlayerState.Idle => GetCachedHash(animTag + "_Idle"),
                PlayerState.Walk => GetCachedHash(animTag + "_Walk"),
                PlayerState.Run => GetCachedHash(animTag + "_Run"),
                _ => throw new InvalidOperationException("Invalid player state")
            };
        }
        
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