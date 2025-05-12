using UnityEngine;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;

namespace ATBMI.Cutscene
{
    public class TalkCutscene : Cutscene
    {
        #region Fields
        
        [Header("Stats")]
        [SerializeField] private bool debugMode;
        [SerializeField] private string debugText;
        [SerializeField] private TextAsset dialogueText;
        
        private bool _isStarted;
        
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
            if (debugMode)
            {
                if (_isStarted) return;
                Debug.Log($"execute talk: {debugText}");
                _isStarted = true;
            }
            
            if (_dialogueManager.IsDialoguePlaying) return;
            DialogueEvents.EnterDialogueEvent(dialogueText);
        }
        
        public override bool IsFinished()
        {
            if (debugMode)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _isStarted = false;
                    return true;
                }

                return false;
            }
           
            return !_dialogueManager.IsDialoguePlaying;
        }
        
        #endregion
    }
}