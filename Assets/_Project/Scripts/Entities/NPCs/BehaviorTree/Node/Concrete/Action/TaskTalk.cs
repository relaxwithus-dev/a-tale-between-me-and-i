using System.Collections.Generic;
using UnityEngine;
using ATBMI.Dialogue;

namespace ATBMI.Entities.NPCs
{
    public class TaskTalk : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterAnimation animation;
        private readonly TextAsset[] dialogueAssets;
        
        private int _talkCount;
        private bool _isDialoguePlay;
        private Vector3 _targetPosition;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsTalk = new()
        {
            { Emotion.Joy, (1, 0.2f, (1, 4)) },
            { Emotion.Trust, (1, 0.3f, (2, 4.5f)) },
            { Emotion.Fear, (1, 0.4f, (1, 2.5f)) },
            { Emotion.Surprise, (1, 0.6f, (1, 2.5f)) },
            { Emotion.Sadness, (1, 0.5f, (1, 4)) },
            { Emotion.Disgust, (1, 0.5f, (1, 4)) },
            { Emotion.Anger, (1, 0.5f, (2, 4.5f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 1f)) }
        };
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsTalkState = new()
        {
            { Emotion.Joy, (1, 0.2f, (0.5f, 4)) },
            { Emotion.Trust, (0, 0.2f, (0.5f, 2)) },
            { Emotion.Fear, (1, 0.4f, (0.5f, 4)) },
            { Emotion.Surprise, (1, 0.2f, (0.5f, 0.5f)) },
            { Emotion.Sadness, (1, 0.3f, (1f, 3)) },
            { Emotion.Disgust, (1, 0.3f, (1f, 3)) },
            { Emotion.Anger, (1, 0.1f, (0.1f, 0.1f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 1f)) }
        };
        
        // Constructor        
        public TaskTalk(CharacterAI character, CharacterAnimation animation, TextAsset[] dialogueAssets, bool isState = false)
        {
            this.character = character;
            this.animation = animation;
            this.dialogueAssets = dialogueAssets;
            
            OverrideEmotionFactors(isState ? _factorsTalkState : _factorsTalk);
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!TrySetupDirection())
                return NodeStatus.Failure;
            
            if (DialogueManager.Instance.IsDialoguePlaying)
                return NodeStatus.Running;
            
            if (_talkCount < dialogueAssets.Length && !_isDialoguePlay)
            {
                DialogueManager.Instance.EnterDialogueMode(dialogueAssets[_talkCount]);
                _talkCount++;
                _isDialoguePlay = true;
                _talkCount = Mathf.Clamp(_talkCount, 0, dialogueAssets.Length - 1);
                return NodeStatus.Running;
            }
            
            animation.TrySetAnimationState(StateTag.IDLE_STATE);
            parentNode.ClearDataContext();
            return NodeStatus.Success;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _isDialoguePlay = false;
            _targetPosition = Vector3.zero;
        }

        private bool TrySetupDirection()
        {
            if (_targetPosition != Vector3.zero)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (target == null)
            {
                Debug.LogWarning("Execute Failure: TaskObserve");
                return false;
            }
            
            _targetPosition = target.transform.position - character.transform.position;
            _targetPosition.Normalize();
            character.LookAt(_targetPosition);
            return true;
        }
    }
}