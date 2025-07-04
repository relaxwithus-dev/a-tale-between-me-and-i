using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class BakeryBT : EmoTrees
    {
        [Header("Attribute")]
        [SerializeField] private float talkRadius;
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var sadnessTexts = characterAI.Data.GetEmotionDialogues(Emotion.Sadness);
            
            Selector tree = new Selector("Bakery BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Sadness", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Talk", new List<Node>
                            {
                                new TaskAnimate(characterAnim, StateTag.CRY_STATE),
                                new CheckTargetInArea(centerPoint, talkRadius, layerMask),
                                new TaskTalk(characterAI, characterAnim, sadnessTexts)
                            }),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Public
                    new Sequence("Public Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Fear", characterTraits, new List<Node>
                        {
                            new TaskAnimate(characterAnim, StateTag.FEAR_STATE),
                            new TaskIdle(characterAI)
                        })
                    })
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}