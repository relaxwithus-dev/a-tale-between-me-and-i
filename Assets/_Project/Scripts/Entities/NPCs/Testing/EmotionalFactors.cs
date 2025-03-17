using System.Collections.Generic;

namespace ATBMI.Entities.NPCs.Testing
{
    public enum NodeType
    {
        Idle,
        MoveToTarget,
        MoveToOrigin,
        Talk,
        TalkExtend,
        Animate,
        Observe,
        JumpBack,
        Push,
        Pull,
        Follow,
        RunAway
    }

    public class EmotionalFactors
    {
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsIdle = new()
        {
            { Emotion.Joy, (1, 0.4f, (0.1f, 0.1f)) },
            { Emotion.Trust, (1, 0.5f, (0.1f, 0.1f)) },
            { Emotion.Fear, (1, 0.5f, (2.5f, 5f)) },
            { Emotion.Surprise, (1, 0.3f, (0.5f, 1f)) },
            { Emotion.Sadness, (1, 0.2f, (0.5f, 5f)) },
            { Emotion.Disgust, (1, 0.2f, (0.5f, 5f)) },
            { Emotion.Anger, (1, 0.3f, (4f, 5f)) },
            { Emotion.Anticipation, (1, 0.5f, (5f, 5f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsMoveToTarget = new()
        {
            { Emotion.Joy, (1, 0.3f, (3f, 7f)) },
            { Emotion.Trust, (1, 0.3f, (3f, 7f)) },
            { Emotion.Fear, (1, 0.5f, (3f, 7f)) },
            { Emotion.Surprise, (1, 0.4f, (2f, 4f)) },
            { Emotion.Sadness, (1, 0.4f, (2f, 4f)) },
            { Emotion.Disgust, (1, 0.4f, (2f, 4f)) },
            { Emotion.Anger, (1, 0.4f, (0.7f, 1.5f)) },
            { Emotion.Anticipation, (1, 0.4f, (2f, 4f)) }
        };

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

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsTalk = new()
        {
            { Emotion.Joy, (1, 0.2f, (0.5f, 4.5f)) },
            { Emotion.Trust, (1, 0.2f, (1f, 3)) },
            { Emotion.Fear, (1, 0.4f, (1, 2.5f)) },
            { Emotion.Surprise, (1, 0.3f, (1f, 3)) },
            { Emotion.Sadness, (1, 0.3f, (1f, 2.5f)) },
            { Emotion.Disgust, (1, 0.3f, (1f, 2.5f)) },
            { Emotion.Anger, (1, 0.5f, (1f, 3.5f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 1f)) }
        };
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsTalkExtend = new()
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

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsAnimate = new()
        {
            { Emotion.Joy, (1, 0.2f, (1.5f, 3f)) },
            { Emotion.Trust, (1, 0.2f, (0.6f, 4f)) },
            { Emotion.Fear, (1, 0.2f, (1.5f, 5f)) },
            { Emotion.Surprise, (1, 0.4f, (0.35f, 3f)) },
            { Emotion.Sadness, (1, 0.2f, (0.35f, 6f)) },
            { Emotion.Disgust, (1, 0.2f, (0.35f, 6f)) },
            { Emotion.Anger, (1, 0.2f, (0.35f, 6f)) },
            { Emotion.Anticipation, (1, 0.3f, (0.35f, 4f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsObserve = new()
        {
            { Emotion.Joy, (1, 0.5f, (2f, 4f)) },
            { Emotion.Trust, (1, 0.5f, (3f, 7f)) },
            { Emotion.Fear, (1, 0.4f, (2f, 4f)) },
            { Emotion.Surprise, (1, 0.5f, (2f, 4f)) },
            { Emotion.Sadness, (1, 0.3f, (1f, 3f)) },
            { Emotion.Disgust, (1, 0.3f, (1.5f, 2f)) },
            { Emotion.Anger, (1, 0.4f, (3f, 7f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 3.5f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsJumpBack = new()
        {
            { Emotion.Joy, (1, 0.6f, (0.3f, 0.7f)) },
            { Emotion.Trust, (1, 0.6f, (0.5f, 1f)) },
            { Emotion.Fear, (1, 0.6f, (0.8f, 1.6f)) },
            { Emotion.Surprise, (1, 0.7f, (0.2f, 0.5f)) },
            { Emotion.Sadness, (1, 0.6f, (0.5f, 1f)) },
            { Emotion.Disgust, (1, 0.6f, (0.5f, 1f)) },
            { Emotion.Anger, (1, 0.6f, (0.6f, 1.1f)) },
            { Emotion.Anticipation, (1, 0.6f, (0.5f, 1f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsPush = new()
        {
            { Emotion.Joy, (1, 0.8f, (0.2f, 0.5f)) },
            { Emotion.Trust, (1, 0.8f, (0.4f, 0.9f)) },
            { Emotion.Fear, (1, 0.8f, (0.6f, 1f)) },
            { Emotion.Surprise, (1, 0.6f, (0.5f, 0.8f)) },
            { Emotion.Sadness, (1, 0.8f, (0.5f, 0.9f)) },
            { Emotion.Disgust, (1, 0.8f, (0.5f, 0.9f)) },
            { Emotion.Anger, (1, 0.8f, (0.3f, 1f)) },
            { Emotion.Anticipation, (1, 0.8f, (0.5f, 0.9f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsPull = new()
        {
            { Emotion.Joy, (1, 0.7f, (0.3f, 1f)) },
            { Emotion.Trust, (1, 0.7f, (0.6f, 1.5f)) },
            { Emotion.Fear, (1, 0.7f, (0.8f, 1.4f)) },
            { Emotion.Surprise, (1, 0.6f, (0.4f, 1.2f)) },
            { Emotion.Sadness, (1, 0.7f, (0.6f, 1.5f)) },
            { Emotion.Disgust, (1, 0.7f, (0.6f, 1.5f)) },
            { Emotion.Anger, (1, 0.7f, (0.3f, 0.6f)) },
            { Emotion.Anticipation, (1, 0.7f, (0.6f, 1.5f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsFollow = new()
        {
            { Emotion.Joy, (1, 0.2f, (3f, 8f)) },
            { Emotion.Trust, (1, 0.1f, (1f, 4.5f)) },
            { Emotion.Fear, (1, 0.6f, (1f, 3f)) },
            { Emotion.Surprise, (1, 0.2f, (3f, 8f)) },
            { Emotion.Sadness, (1, 0.5f, (1f, 4.5f)) },
            { Emotion.Disgust, (1, 0.5f, (1f, 4.5f)) },
            { Emotion.Anger, (1, 0.4f, (5f, 12f)) },
            { Emotion.Anticipation, (1, 0.5f, (1f, 4.5f)) }
        };

        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsRunAway = new()
        {
            { Emotion.Joy, (1, 0.7f, (3f, 6f)) },
            { Emotion.Trust, (1, 0.7f, (4f, 9f)) },
            { Emotion.Fear, (1, 0.2f, (0.5f, 3f)) },
            { Emotion.Surprise, (1, 0.7f, (3f, 6f)) },
            { Emotion.Sadness, (1, 0.6f, (4f, 9f)) },
            { Emotion.Disgust, (1, 0.6f, (4f, 9f)) },
            { Emotion.Anger, (1, 0.3f, (4f, 9f)) },
            { Emotion.Anticipation, (1, 0.6f, (4f, 9f)) }
        };
        
        // Getter
        public Dictionary<Emotion, (float plan, float risk, (float, float) time)> GetFactors(NodeType node)
        {
            return node switch
            {
                NodeType.Idle => _factorsIdle,
                NodeType.MoveToTarget => _factorsMoveToTarget,
                NodeType.MoveToOrigin => _factorsMoveToOrigin,
                NodeType.Talk => _factorsTalk,
                NodeType.TalkExtend => _factorsTalkExtend,
                NodeType.Animate => _factorsAnimate,
                NodeType.Observe => _factorsObserve,
                NodeType.JumpBack => _factorsJumpBack,
                NodeType.Push => _factorsPush,
                NodeType.Pull => _factorsPull,
                NodeType.Follow => _factorsFollow,
                NodeType.RunAway => _factorsRunAway,
                _ => null
            };
        }
    }
}