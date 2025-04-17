using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class DewaParentBT : EmoTrees
    {
        protected override Node SetupTree()
        {
            var anticipationText = GetTextAssets(Emotion.Anticipation);
            
            Selector tree = new Selector("Orang Tua Dewa", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new Sequence("Intimate Zone", new List<Node>
                {
                    new CheckTargetInProxemics(centerPoint, zoneDetails[0].Radius, layerMask),
                    new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                    {
                        new TaskTalk(characterAI, anticipationText),
                        new TaskIdle(characterAI)
                    })
                })
            });
            
            return tree;
        }
    }
}
