using System;
using System.Collections;
using UnityEngine;
using ATBMI.Audio;

namespace ATBMI.Entities.Player
{
    [RequireComponent(typeof(AnimateEventReceiver))]
    public class PlayerAnimation : Animatable
    {
        private bool _isInteractiveAnimation;
        
        // Reference
        private PlayerController _playerController;
        private AnimateEventReceiver _eventReceiver;
        
        // Unity Callbacks
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _playerController = GetComponentInParent<PlayerController>();
            _eventReceiver = GetComponent<AnimateEventReceiver>();
        }

        protected override void SubsEvent()
        {
            base.SubsEvent();
            _eventReceiver.OnStepDown += () => AudioManager.Instance.PlayAudio(Musics.SFX_Footstep);
        }

        private void Update()
        {
            AnimationStateHandler();
        }
        
        // Core
        private void AnimationStateHandler()
        {
            if (_isInteractiveAnimation) return;
            var state = GetState();
            
            if (state == currentState) return;
            Debug.Log(state);
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }
        
        public override bool TrySetAnimationState(string state, bool isOnce = false)
        {
            var stateName = state;
            var tagPrefix = _playerController.Data.PlayerAnimationTag;
            if (!state.StartsWith(tagPrefix + "_"))
            {
                stateName = tagPrefix + "_" + state;
            }
            
            if (!animationHashes.ContainsKey(stateName))
            {
                Debug.LogWarning($"animation {stateName} isn't exist");
                return false;
            }
            
            StartCoroutine(PlayOnceRoutine(stateName, isOnce));
            return true;
        }
        
        public override float GetAnimationTime() => animator.GetCurrentAnimatorClipInfo(0).Length;
        
        private IEnumerator PlayOnceRoutine(string stateName, bool isOnce)
        {
            _isInteractiveAnimation = true;
            currentState = GetCachedHash(stateName);
            animator.CrossFade(currentState, 0, 0);

            if (!isOnce) yield break;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
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
    }
}