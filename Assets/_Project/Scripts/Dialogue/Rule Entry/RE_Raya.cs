using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    public class RE_Raya : RuleEntry
    {
        #region Dialogue Rules Parameters

        [Space(20)]
        [Header("Dialogue Rules")]
        [SerializeField] private int visitedCount;
        #endregion
        #region Dialogue Text Assets
        [Space(20)]
        [Header("Dialogue Text Assets")]
        public TextAsset onTalk;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            isPlayerInRange = false;
            isDialogueAboutToStart = false;

            visitedCount = 0;
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }

        public override void OnEnterDialogue(TextAsset defaultDialogue)
        {
            if (!isDialogueAboutToStart && isPlayerInRange)
            {
                // TODO: change to default dialogue (use rules if default dialogue > 1, eg. default dialogue ch1, ch2, ch3...)
                base.EnterDialogue(this, defaultDialogue);
            }
        }
        
        public override void OnEnterItemDialogue(TextAsset itemDialogue)
        {
           if (!isDialogueAboutToStart && isPlayerInRange)
            {
                base.EnterDialogue(this, itemDialogue);
            }
        }

        public override void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            base.EnterDialogueWithInkJson(InkJson);
        }

    }
}
