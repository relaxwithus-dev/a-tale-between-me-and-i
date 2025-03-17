using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class DewaParentBT : EmoTrees
    {
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Orang Tua Dewa", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new Sequence("Intimate Zone", new List<Node>
                {
                    new CheckTargetInProxemics(centerPoint, zoneDetails[0].Radius, layerMask),
                    new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                    {
                        new TaskTalk(characterAI, "hi nak, ada apa?"),
                        new TaskIdle(characterAI)
                    })
                })
            });
            
            return tree;
        }
    }
}
