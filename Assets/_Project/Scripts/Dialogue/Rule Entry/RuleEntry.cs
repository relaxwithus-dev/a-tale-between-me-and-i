using ATBMI.Entities.NPCs;
using UnityEngine;

namespace ATBMI.Dialogue
{
    public abstract class RuleEntry : MonoBehaviour
    {
        [Header("Params")]
        public CharacterAI npc;
        public Transform playerEntryPoint;
        public Animator emoteAnimator;
        public bool isPlayerInRange;
        public bool isDialogueAboutToStart;

        public abstract void OnTriggerEnter2D(Collider2D other);
        public abstract void OnTriggerExit2D(Collider2D other);
        public abstract void EnterDialogue();

        public virtual void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            DialogueManager.Instance.EnterDialogueMode(InkJson, emoteAnimator);

            isDialogueAboutToStart = false;
        }
    }
}
