using UnityEngine;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;

namespace ATBMI.Cutscene
{
    public class TalkCutscene : Cutscene
    {
        #region Fields
        
        [Header("Stats")]
        [SerializeField] private TextAsset dialogueText;
        
        // Reference
        private DialogueManager _dialogueManager;

        #endregion

        #region Methods
        
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _dialogueManager = DialogueManager.Instance;
        }
        
        public override void Execute()
        {
            if (_dialogueManager.IsDialoguePlaying) return;
            DialogueEvents.EnterDialogueEvent(dialogueText);
        }

        public override bool IsFinished() => !_dialogueManager.IsDialoguePlaying;
        
        #endregion
    }
}