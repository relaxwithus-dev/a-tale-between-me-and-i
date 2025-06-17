using UnityEngine;
using ATBMI.Gameplay.Event;

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
    
    private void PlayDialogueAnim(string speaker, string expression)
    {
        // get the animation state
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // check is there any animation called expressionValue
        hasAnimation = anim.HasState(0, Animator.StringToHash(expression)); 
        
        // play anim
        if (hasAnimation && !stateInfo.IsName(expression))
        {
            anim.Play(expression);
        }
    }
    
    private void StopDialogueAnim(string speaker)
    {
        anim.Play("A_StopDialogue");
    }
}
