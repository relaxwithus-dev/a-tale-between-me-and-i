using System.Collections.Generic;
using UnityEngine;
using ATBMI.Dialogue;

namespace ATBMI.Entities.NPCs
{
    public class TaskTalk : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterState state;
        private readonly TextAsset[] dialogueAssets;
        private readonly Animator emoteAnim;

        private int _talkCount;
                
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
        public TaskTalk(CharacterAI character, TextAsset[] dialogueAssets, Animator emoteAnim)
        {
            this.character = character;
            this.emoteAnim = emoteAnim;
            this.dialogueAssets = dialogueAssets;
            
            OverrideEmotionFactors(_factorsTalk);
        }
        
        public TaskTalk(CharacterAI character, CharacterState state, TextAsset[] dialogueAssets, Animator emoteAnim)
        {
            this.state = state;
            this.character = character;
            this.emoteAnim = emoteAnim;
            this.dialogueAssets = dialogueAssets;
            
            OverrideEmotionFactors(_factorsTalkState);
        } 
        
        // Core
        public override NodeStatus Evaluate()
        {
            Debug.Log("Execute: TaskTalk");
            var targetState = state is CharacterState.Idle ? CharacterState.Talk : state;
            
            DialogueManager.Instance.EnterDialogueMode(dialogueAssets[_talkCount], emoteAnim);
            _talkCount = Mathf.Clamp(_talkCount++, 0, dialogueAssets.Length - 1);
            character.ChangeState(targetState);
            
            return NodeStatus.Success;
        }
    }
}