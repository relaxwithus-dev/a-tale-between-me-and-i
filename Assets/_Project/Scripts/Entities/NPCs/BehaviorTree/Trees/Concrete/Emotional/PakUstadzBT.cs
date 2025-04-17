using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class PakUstadzBT : EmoTrees
    { 
        [Header("Attribute")] 
        [SerializeField] private float jumpPower = 0.3f;
        [SerializeField] private float jumpDuration = 0.15f;
        [SerializeField] private float talkRadius;
        [SerializeField] private Transform targetPoint;
        
        protected override Node SetupTree()
        {
            var surpriseText = GetTextAssets(Emotion.Surprise);
            
            Selector tree = new Selector("Pak Ustadz BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Intimate
                    new Sequence("Intimate Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Anger", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Jump Back",new List<Node>
                            {
                                new CheckDirection(characterAI),
                                new TaskJumpBack(characterAI, jumpPower, jumpDuration),
                                new TaskTalk(characterAI, surpriseText)
                            }),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Talk", new List<Node>
                            {
                                new CheckTargetInArea(centerPoint, talkRadius, layerMask),
                                new TaskTalk(characterAI, defaultTexts)
                            }),
                            new TaskIdle(characterAI)
                        })
                    })
                }),
                new Sequence("Move", new List<Node>
                {
                    new CheckReachTarget(characterAI, targetPoint),
                    new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, targetPoint),
                    new TaskIdle(characterAI)
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}