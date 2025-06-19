using UnityEngine;
using ATBMI.Gameplay.Event;

public class DialogueExpressionController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private AnimatorStateInfo _stateInfo;
    private bool _hasAnimation;
    
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
        _stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        _hasAnimation = anim.HasState(0, Animator.StringToHash(expression)); 
        
        if (_hasAnimation && !_stateInfo.IsName(expression))
        {
            anim.Play(expression);
        }
    }
    
    // TODO: Optimize dgn lgsg panggil animation buat set idle
    private void StopDialogueAnim(string speaker)
    {
        anim.Play("A_StopDialogue");
    }
}
