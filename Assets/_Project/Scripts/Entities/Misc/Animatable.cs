using System;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Entities
{
    [RequireComponent(typeof(Animator))]
    public abstract class Animatable : MonoBehaviour
    {
        // Global fields
        protected int currentState;
        protected readonly Dictionary<string, int> animationHashes = new();
        
        protected Animator animator;

        // Unity Callbacks
        private void Awake()
        {
            InitOnAwake();
        }

        protected virtual void OnEnable()
        {
            SubsEvent();
        }

        private void OnDisable()
        {
            UnsubsEvent();
        }

        private void Start()
        {
            InitAnimationHash();
        }

        // Initialize
        protected virtual void InitOnAwake()
        {
            animator = GetComponent<Animator>();
        }
        
        protected virtual void SubsEvent()
        {
            DialogueEvents.PlayDialogueAnim += OnPlayDialogue;
            DialogueEvents.StopDialogueAnim += StopDialogueAnim;
        }

        protected virtual void UnsubsEvent()
        {
            DialogueEvents.PlayDialogueAnim -= OnPlayDialogue;
            DialogueEvents.StopDialogueAnim -= StopDialogueAnim;
        }
        
        public void InitAnimationHash()
        {
            animationHashes.Clear();
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (!animationHashes.ContainsKey(clip.name))
                    animationHashes[clip.name] = Animator.StringToHash(clip.name);
            }
        }
        
        // Core
        public abstract bool TrySetAnimationState(string state, bool isOnce = false);
        public abstract float GetAnimationTime();
        
        protected int GetCachedHash(string animName)
        {
            if (animationHashes.TryGetValue(animName, out var hash))
                return hash;
            
            hash = Animator.StringToHash(animName);
            animationHashes[animName] = hash;
            return hash;
        }
        
        private void OnPlayDialogue(string speaker, string expression) => TrySetAnimationState(expression);
        protected virtual void StopDialogueAnim(string speaker) => TrySetAnimationState("Idle");
    }
}