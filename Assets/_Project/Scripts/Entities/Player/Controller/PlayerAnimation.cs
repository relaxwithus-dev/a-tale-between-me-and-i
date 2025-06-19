using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Audio;

namespace ATBMI.Entities.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimateEventReceiver))]
    public class PlayerAnimation : MonoBehaviour, IAnimatable
    {
        #region Fields & Properties
        
        private int _currentState;
        private bool _isInteractiveAnimation;
        private readonly Dictionary<string, int> _animationHashes = new();
        
        // Reference
        private PlayerController _playerController;
        private AnimateEventReceiver _eventReceiver;
        private Animator _playerAnim;
        
        #endregion
        
        #region Unity Methods
        
        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _eventReceiver = GetComponent<AnimateEventReceiver>();
            _playerAnim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _eventReceiver.OnStepDown += PlayFootstepSound;
        }
        
        private void OnDisable()
        {
            _eventReceiver.OnStepDown -= PlayFootstepSound;
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
            foreach (var clip in _playerAnim.runtimeAnimatorController.animationClips)
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
            _playerAnim.CrossFade(state, 0, 0);
            _currentState = state;
        }
        
        public bool TrySetAnimationState(string state)
        {
            var stateName = _playerController.Data.PlayerAnimationTag + "_" + state;
            if (!_animationHashes.ContainsKey(stateName))
            {
                Debug.LogWarning($"animation {stateName} isn't exist");
                return false;
            }

            StartCoroutine(PlayAnimationRoutine(stateName));
            return true;
        }
        
        public float GetAnimationTime() => _playerAnim.GetCurrentAnimatorClipInfo(0).Length;
        
        private IEnumerator PlayAnimationRoutine(string stateName)
        {
            _isInteractiveAnimation = true;
            _currentState = GetCachedHash(stateName);
            _playerAnim.CrossFade(_currentState, 0, 0);
            
            yield return new WaitForSeconds(_playerAnim.GetCurrentAnimatorClipInfo(0).Length);
            _isInteractiveAnimation = false;
        }
        
        private int GetState()
        {
            var animTag = _playerController.Data.PlayerAnimationTag;
            return _playerController.PlayerState switch
            {
                EntitiesState.Idle => GetCachedHash(animTag + "_Idle"),
                EntitiesState.Walk => GetCachedHash(animTag + "_Walk"),
                EntitiesState.Run => GetCachedHash(animTag + "_Run"),
                _ => throw new InvalidOperationException("Invalid player state")
            };
        }
        
        // Helpers
        private void PlayFootstepSound() => AudioManager.Instance.PlayAudio(Musics.SFX_Footstep);
        
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