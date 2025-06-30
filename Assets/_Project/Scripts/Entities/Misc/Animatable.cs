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
        
        private void UnsubsEvent()
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
        public abstract bool TrySetAnimationState(string state, string speaker = "", bool isOnce = false);

        public float GetAnimationTime() => animator.GetCurrentAnimatorClipInfo(0).Length;
        protected bool CheckMatchSpeaker(string speaker, string name) => speaker != "" && speaker == name;
        
        protected int GetCachedHash(string animName)
        {
            if (animationHashes.TryGetValue(animName, out var hash))
                return hash;
            
            hash = Animator.StringToHash(animName);
            animationHashes[animName] = hash;
            return hash;
        }
        
        protected string GetStateName(string state, string tagPrefix)
        {
            var stateName = state;
            if (state.StartsWith(tagPrefix + "_")) return stateName;
            
            stateName = tagPrefix + "_" + state;
            return stateName;

        }
        
        // Event
        private void OnPlayDialogue(string speaker, string expression) => TrySetAnimationState(expression, speaker);
        protected virtual void StopDialogueAnim(string speaker) => TrySetAnimationState("Idle", speaker);
    }
}