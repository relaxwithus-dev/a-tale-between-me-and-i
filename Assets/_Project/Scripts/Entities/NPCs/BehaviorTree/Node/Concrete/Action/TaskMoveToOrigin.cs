using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToOrigin : TaskMoveBase
    {
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsMoveToOrigin = new()
        {
            { Emotion.Joy, (1, 0.2f, (2.5f, 6f)) },
            { Emotion.Trust, (1, 0.2f, (2.5f, 6f)) },
            { Emotion.Fear, (1, 0.3f, (2.5f, 6f)) },
            { Emotion.Surprise, (1, 0.3f, (1.5f, 3f)) },
            { Emotion.Sadness, (1, 0.3f, (1.5f, 3f)) },
            { Emotion.Disgust, (1, 0.3f, (1.5f, 3f)) },
            { Emotion.Anger, (1, 0.3f, (0.5f, 1.5f)) },
            { Emotion.Anticipation, (1, 0.3f, (1.5f, 3f)) }
        };
        
        // Constructor
        public TaskMoveToOrigin(CharacterAI character, CharacterData data, bool isWalk) : base(character, data, isWalk)
        {
            OverrideEmotionFactors(_factorsMoveToOrigin);
        }
        
        // Core
        protected override bool TrySetupTarget()
        {
            if (targetPosition != Vector3.zero)
                return true;

            var originPoint = (Vector3)GetData(ORIGIN_KEY);
            if (originPoint == Vector3.zero)
                return false;
            
            targetPosition = new Vector3(originPoint.x,
                character.transform.position.y,
                character.transform.position.z);

            return true;
        }
        
        protected override void WhenReachTarget()
        {
            base.WhenReachTarget();
            
            var direction = character.Direction * -1f;
            character.LookAt(direction);
            parentNode.ClearData(ORIGIN_KEY);
        }
    }
}