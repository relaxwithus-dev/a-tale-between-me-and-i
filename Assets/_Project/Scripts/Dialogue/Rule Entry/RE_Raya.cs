using System.Collections;
using System.Collections.Generic;
using ATMBI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
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

        public override void EnterDialogue()
        {
            if (!isDialogueAboutToStart)
            {
                isDialogueAboutToStart = true;
                PlayerEventHandler.MoveToPlayerEvent(this, onTalk, playerEntryPoint.position.x, npc.isFacingRight);
            }
        }

        public override void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            base.EnterDialogueWithInkJson(InkJson);
        }
    }
}
