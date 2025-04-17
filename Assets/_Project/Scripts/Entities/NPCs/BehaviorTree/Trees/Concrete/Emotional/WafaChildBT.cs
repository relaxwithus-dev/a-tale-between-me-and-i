using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class WafaChildBT : EmoTrees
    {
        [Header("Attribute")] 
        [SerializeField] private float followDuration = 6f;
        [SerializeField] private Transform[] wayPoints;
        
        [Space]
        [SerializeField] private CharacterManager characterManager;
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Luna BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Joy and Anticipation", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Follow", new List<Node>
                            {
                                new TaskFollow(characterAI, characterAI.Data, followDuration),
                                new TaskTalk(characterAI, defaultTexts)
                            }),
                            new TaskTalk(characterAI, defaultTexts),
                            new TaskObserve(characterAI, characterAnim, zoneDetails[1].Radius),
                            new TaskIdle(characterAI)
                        })
                    })
                }),
                new Sequence("Move", new List<Node>
                {
                    new CheckFatigue(characterManager),
                    new TaskPatrol(characterAI, characterManager, characterAI.Data, wayPoints)
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}