using ATBMI.Gameplay.Event;
using UnityEngine;

public class DialogueExpressionController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private AnimatorStateInfo stateInfo;
    private bool hasAnimation;

    private void OnEnable()
    {
        DialogueEvents.PlayDialogueAnim += PlayDialogueAnim;
        DialogueEvents.StopDialogueAnim += StopDialogueAnim;
    }
    
    private void OnDisable()
    {
        DialogueEvents.PlayDialogueAnim -= PlayDialogueAnim;
        DialogueEvents.StopDialogueAnim -= StopDialogueAnim;
    }
    
    private void PlayDialogueAnim(string expressionValue)
    {
        // get the animation state
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // check is there any animation called expressionValue
        hasAnimation = anim.HasState(0, Animator.StringToHash(expressionValue)); 

        // play anim
        if (hasAnimation && !stateInfo.IsName(expressionValue))
        {
            anim.Play(expressionValue);
        }
    }
    
    private void StopDialogueAnim()
    {
        anim.Play("A_StopDialogue");
    }
}
