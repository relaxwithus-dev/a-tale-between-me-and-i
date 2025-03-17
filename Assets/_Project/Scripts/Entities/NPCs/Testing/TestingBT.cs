using System.Collections.Generic;
using ATBMI.Entities.NPCs.Testing;

namespace ATBMI.Entities.NPCs
{
    public class TestingBT : EmoTrees
    {
        private EmotionalFactors _emotionalFactors;
        protected override Node SetupTree()
        {
            _emotionalFactors = new EmotionalFactors();

            EmotionalSelector testing = new EmotionalSelector("Testing BT", characterTraits, new List<Node>
            {
                new FactorLeaf(NodeType.Idle, _emotionalFactors),
                // new FactorLeaf(NodeType.MoveToTarget, _emotionalFactors),
                // new FactorLeaf(NodeType.MoveToOrigin, _emotionalFactors),
                new FactorLeaf(NodeType.Talk, _emotionalFactors),
                new FactorLeaf(NodeType.Animate, _emotionalFactors),
                new FactorLeaf(NodeType.Observe, _emotionalFactors),
                new SequenceWeight("Jump Back", new List<Node>
                {
                    new FactorLeaf(NodeType.JumpBack, _emotionalFactors),
                    new FactorLeaf(NodeType.TalkExtend, _emotionalFactors)
                }),
                new SequenceWeight("Pull", new List<Node>
                {
                    new FactorLeaf(NodeType.Pull, _emotionalFactors),
                    new FactorLeaf(NodeType.TalkExtend, _emotionalFactors),
                }),
                new SequenceWeight("Push", new List<Node>
                {
                    new FactorLeaf(NodeType.MoveToTarget, _emotionalFactors),
                    new FactorLeaf(NodeType.Pull, _emotionalFactors),
                    new FactorLeaf(NodeType.TalkExtend, _emotionalFactors),
                    new FactorLeaf(NodeType.MoveToOrigin, _emotionalFactors)
                }),
                new SequenceWeight("Follow", new List<Node>
                {
                    new FactorLeaf(NodeType.Follow, _emotionalFactors),
                    new FactorLeaf(NodeType.TalkExtend, _emotionalFactors)
                }),
                new FactorLeaf(NodeType.RunAway, _emotionalFactors)
            });

            return testing;
        }
    }
}