using UnityEngine;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;

namespace ATBMI.Cutscene
{
    public class TalkCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")]
        [SerializeField] private TextAsset dialogueText;
        
        private bool _isStartDialogue;
        
        // Reference
        private DialogueManager _dialogueManager;
        
        #endregion
        
        #region Methods

        private void OnEnable()
        {
            DialogueEvents.OnExitDialogue += FinishDialogue;
        }
        
        private void OnDisable()
        {
            DialogueEvents.OnExitDialogue -= FinishDialogue;
        }
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            _isStartDialogue = false;
            _dialogueManager = DialogueManager.Instance;
        }
        
        public override void Execute()
        {
            if (_dialogueManager.IsDialoguePlaying) return;
            _isStartDialogue = true;
            _dialogueManager.EnterDialogueMode(dialogueText);
        }

        private void FinishDialogue()
        {
            if (!_isStartDialogue) return;
            isFinishStep = true;
        }

        #endregion
    }
}