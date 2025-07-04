using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class DewaParentBT : EmoTrees
    {
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var data = characterAI.Data;
            var anticipationTexts = data.GetEmotionDialogues(Emotion.Anticipation);
            
            Selector tree = new Selector("Orang Tua Dewa", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new Sequence("Intimate Zone", new List<Node>
                {
                    new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                    new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                    {
                        new TaskTalk(characterAI, characterAnim, anticipationTexts),
                        new TaskIdle(characterAI)
                    })
                })
            });
            
            return tree;
        }
    }
}
