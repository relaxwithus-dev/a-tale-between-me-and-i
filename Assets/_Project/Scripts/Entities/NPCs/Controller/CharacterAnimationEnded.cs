using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CharacterAnimationEnded : StateMachineBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private CharacterAI characterAI;
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            characterAI.ChangeState(CharacterState.Idle);
        }
    }
}