using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskTalk : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterState state;
        private readonly string dialogueText;
        
        private bool _isDialogueEnded;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsTalk = new()
        {
            { Emotion.Joy, (1, 0.2f, (0.5f, 4.5f)) },
            { Emotion.Trust, (1, 0.2f, (1f, 3)) },
            { Emotion.Fear, (1, 0.4f, (0.5f, 4)) },
            { Emotion.Surprise, (1, 0.3f, (1f, 3)) },
            { Emotion.Sadness, (1, 0.3f, (1f, 2.5f)) },
            { Emotion.Disgust, (1, 0.3f, (1f, 2.5f)) },
            { Emotion.Anger, (1, 0.5f, (1f, 3.5f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 1f)) }
        };
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsTalkState = new()
        {
            { Emotion.Joy, (1, 0.2f, (0.5f, 4)) },
            { Emotion.Trust, (1, 0.2f, (1f, 3)) },
            { Emotion.Fear, (1, 0.4f, (0.5f, 4)) },
            { Emotion.Surprise, (1, 0.3f, (1f, 3)) },
            { Emotion.Sadness, (1, 0.3f, (1f, 3)) },
            { Emotion.Disgust, (1, 0.3f, (1f, 3)) },
            { Emotion.Anger, (1, 0.2f, (0.2f, 0.4f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 1f)) }
        };
        
        // Constructor        
        public TaskTalk(CharacterAI character, string dialogueText)
        {
            this.character = character;
            this.dialogueText = dialogueText;
            
            OverrideEmotionFactors(_factorsTalk);
        }
        
        public TaskTalk(CharacterAI character, CharacterState state, string dialogueText)
        {
            this.character = character;
            this.dialogueText = dialogueText;
            this.state = state;
            
            OverrideEmotionFactors(_factorsTalkState);
        } 
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isDialogueEnded)
            {
                return NodeStatus.Success;
            }
            
            Debug.Log($"Execute: TaskDialogue( {dialogueText} )");
            var targetState = state is CharacterState.Idle ? CharacterState.Talk : state;
            character.ChangeState(targetState);
            _isDialogueEnded = true;
            return NodeStatus.Running;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _isDialogueEnded = false;
        }
    }
}