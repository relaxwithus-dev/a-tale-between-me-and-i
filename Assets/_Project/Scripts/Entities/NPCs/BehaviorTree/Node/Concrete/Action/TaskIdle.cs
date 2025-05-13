using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class TaskIdle : LeafWeight
    {
        private readonly CharacterAI character;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsIdle = new()
        {
            { Emotion.Joy, (1, 0.4f, (2f, 2f)) },
            { Emotion.Trust, (1, 0.6f, (2.5f, 2.5f)) },
            { Emotion.Fear, (1, 0.5f, (2.5f, 5f)) },
            { Emotion.Surprise, (1, 0.3f, (1.5f, 2f)) },
            { Emotion.Sadness, (1, 0.2f, (2f, 2.5f)) },
            { Emotion.Disgust, (1, 0.2f, (2f, 2.5f)) },
            { Emotion.Anger, (1, 0.3f, (5f, 5f)) },
            { Emotion.Anticipation, (1, 0.5f, (5f, 5f)) }
        };
        
        // Constructor
        public TaskIdle(CharacterAI character)
        {
            this.character = character;
            OverrideEmotionFactors(_factorsIdle);
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            character.ChangeState(EntitiesState.Idle);
            return NodeStatus.Success;
        }
    }
}