using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class PemabukBT : EmoTrees
    {
        protected override Node SetupTree()
        {
            Selector tree = new Selector(rootName, new List<Node>
            {
                new TaskIdle(characterAI)
            });

            return tree;
        }
    }
}