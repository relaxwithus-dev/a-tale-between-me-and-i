using System;
using UnityEngine;

namespace ATBMI
{
    public abstract class RuleEntry : MonoBehaviour
    {
        [Header("Params")]
        public NPC npc;
        public Transform playerEntryPoint;
        public bool isPlayerInRange;
        public bool isDialogueAboutToStart;

        public abstract void OnTriggerEnter2D(Collider2D other);
        public abstract void OnTriggerExit2D(Collider2D other);
        public abstract void EnterDialogue();

        public virtual void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            DialogueManager.Instance.EnterDialogueMode(InkJson);

            isDialogueAboutToStart = false;
        }
    }
}
