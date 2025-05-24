namespace ATBMI.Entities.NPCs
{
    public class FactorLeaf : LeafWeight
    {
        private readonly EmotionalFactors factors;

        public FactorLeaf(NodeType node, EmotionalFactors factors)
        {
            // nodeName = node.ToString();
            this.factors = factors;
            OverrideEmotionFactors(this.factors.GetFactors(node));
        }

        public override NodeStatus Evaluate()
        {
            return NodeStatus.Success;
        }
    }
}