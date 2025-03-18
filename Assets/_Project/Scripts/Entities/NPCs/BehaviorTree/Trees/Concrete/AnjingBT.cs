using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class AnjingBT : Trees
    {
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Anjing", new List<Node>
            {
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}