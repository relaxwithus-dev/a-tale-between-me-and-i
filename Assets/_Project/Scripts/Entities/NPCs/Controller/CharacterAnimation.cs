using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CharacterAnimation : Animatable
    {
        // Fields
        private CharacterAI _characterAI;
        
        // Unity Callbacks
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _characterAI = GetComponentInParent<CharacterAI>();
        }
        
        // Core
        public override bool TrySetAnimationState(string state, bool isOnce = false)
        {
            var stateName = _characterAI.Data.AnimationTag + "_" + state;
            if (!animationHashes.ContainsKey(stateName))
            {
                Debug.LogWarning("Animation isn't exists");
                return false;
            }
            
            currentState = GetCachedHash(stateName);
            animator.CrossFade(currentState, 0, 0);
            return true;
        }
        
        public override float GetAnimationTime() => animator.GetCurrentAnimatorClipInfo(0).Length;
    }
}