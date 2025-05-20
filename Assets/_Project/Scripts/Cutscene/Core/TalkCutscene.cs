using UnityEngine;
using ATBMI.Dialogue;
using ATBMI.Entities;
using ATBMI.Gameplay.Event;

namespace ATBMI.Cutscene
{
    public class TalkCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")]
        [SerializeField] private TextAsset dialogueText;
        
        
        // Reference
        private DialogueManager _dialogueManager;
        
        #endregion
        
        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
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