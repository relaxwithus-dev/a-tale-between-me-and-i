namespace ATBMI.Entities.NPCs
{
    public class Inverter : Node
    {
        public Inverter(string nodeName) : base(nodeName) { }

        public override NodeStatus Evaluate()
        {
            switch (childNodes[0].Evaluate())
            {
                case NodeStatus.Running:
                    return NodeStatus.Running;
                case NodeStatus.Failure:
                    return NodeStatus.Success;
                case NodeStatus.Success:
                default:
                    return NodeStatus.Failure;;
            }
        }
    }
}