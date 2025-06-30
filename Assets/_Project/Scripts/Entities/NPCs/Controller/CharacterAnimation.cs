using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CharacterAnimation : Animatable
    {
        // Fields
        private string _charaState;
        private CharacterAI _characterAI;
        
        // Unity Callbacks
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _characterAI = GetComponentInParent<CharacterAI>();
        }
        
        // Core
        public override bool TrySetAnimationState(string state, string speaker = "", bool isOnce = false)
        {
            var data = _characterAI.Data;
            if (!CheckMatchSpeaker(speaker, data.CharacterName)) 
                return false;
            
            var stateName = GetStateName(state, data.AnimationTag);
            if (!animationHashes.ContainsKey(stateName))
            {
                Debug.LogWarning("Animation isn't exists");
                return false;
            }
            
            currentState = GetCachedHash(stateName);
            animator.CrossFade(currentState, 0, 0);
            return true;
        }

        protected override void StopDialogueAnim(string speaker)
        {
            var data = _characterAI.Data;
            if (speaker != "" && speaker != data.CharacterName) return;
            
            base.StopDialogueAnim(speaker);
        }
    }
}