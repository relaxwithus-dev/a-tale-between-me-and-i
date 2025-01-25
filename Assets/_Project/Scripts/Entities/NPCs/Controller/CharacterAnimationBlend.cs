using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CharacterAnimationBlend : StateMachineBehaviour
    {
        [Header("Properties")]
        [SerializeField] private string animationName;
        [SerializeField] private int animationLength;
        [SerializeField] private float shuffleTime;
                
        private bool _isShuffled;
        private float _currentTime;
        
        // Cached properties
        private readonly float StateMaxTime = 0.98f;
        private readonly float StateMinTime = 0.02f;
        private readonly string Blend = "Blend";
        
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetAnimation(animator);
        }
        
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_isShuffled)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime >= shuffleTime && stateInfo.normalizedTime % 1 < StateMinTime)
                {
                    _isShuffled = true;
                    _currentTime = 0f;
                    var tempAnimation = Random.Range(0, animationLength);
                    animator.SetFloat(Blend, tempAnimation);
                }
            }
            else if (stateInfo.normalizedTime % 1 > StateMaxTime)
            {
                ResetAnimation(animator);
            }
        }
        
        private void ResetAnimation(Animator animator)
        {
            _isShuffled = false;
            _currentTime = 0f;
            animator.SetFloat(Blend, 0);
        }
    }
}
