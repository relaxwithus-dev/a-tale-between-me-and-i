namespace ATBMI.Entities.NPCs
{
    public class UntilFail : Node
    {
        public UntilFail(string nodeName) : base(nodeName) { }

        public override NodeStatus Evaluate()
        {
            if (childNodes[0].Evaluate() == NodeStatus.Failure)
            {
                Reset();
                return NodeStatus.Failure;
            }

            return NodeStatus.Running;
        }
    }
}