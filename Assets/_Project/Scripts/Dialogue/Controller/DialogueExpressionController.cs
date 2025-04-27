using System.Collections;
using ATBMI.Entities.NPCs;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Event;
using UnityEngine;

public class DialogueExpressionController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // For player
    [SerializeField] private CharacterAI characterAI;           // For NPC
    [SerializeField] private Animator anim;

    private AnimatorStateInfo stateInfo;
    private bool hasAnimation;
    private string characterName;

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

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        if (playerController != null)
        {
            characterName = playerController.Data.PlayerName;
        }
        else if (characterAI != null)
        {
            characterName = characterAI.Data.CharacterName;
        }
        else
        {
            Debug.LogError(gameObject.name + " characterInfoRaw does not implement ICharacterInfo!");
        }
    }

    private void PlayDialogueAnim(string speakerName, string expressionValue)
    {
        if (speakerName != characterName) return; // Only react if this is the right speaker!

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

    private void StopDialogueAnim(string speakerName)
    {
        if (speakerName != characterName) return; // Only react if this is the right speaker!

        anim.Play("A_StopDialogue");
    }
}
