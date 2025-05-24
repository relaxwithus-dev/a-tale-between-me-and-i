using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class WafaChildBT : EmoTrees
    {
        [Header("Attribute")] 
        [SerializeField] private float followDuration = 6f;
        [SerializeField] private float moveStamina;
        [SerializeField] private Transform[] wayPoints;
        
        [Space]
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var defaultTexts = characterAI.Data.GetDefaultDialogues();
            var joyTexts = characterAI.Data.GetEmotionDialogues(Emotion.Joy);
            
            CheckFatigue checkFatigue = new CheckFatigue(moveStamina);
            
            Selector tree = new Selector("Wafa Child BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Joy and Anticipation", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Follow", new List<Node>
                            {
                                new TaskFollow(characterAI, characterAI.Data, followDuration),
                                new TaskTalk(characterAI, characterAnim, joyTexts, isState: true)
                            }),
                            new TaskTalk(characterAI, characterAnim, defaultTexts),
                            new TaskObserve(characterAI, characterAnim, zoneDetails[1].Radius),
                            new TaskIdle(characterAI)
                        })
                    })
                }),
                new Sequence("Patrol", new List<Node>
                {
                    checkFatigue,
                    new TaskPatrol(characterAI, checkFatigue, wayPoints)
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}