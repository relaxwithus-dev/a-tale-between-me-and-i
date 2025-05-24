using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class PemabukBT : EmoTrees
    {
        [Header("Attribute")] 
        [SerializeField] private float pushForce; 
        [SerializeField] private float pushDelay;
        [SerializeField] private float moveStamina;
        [SerializeField] private Transform[] wayPoints;

        [Space]
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var data = characterAI.Data;
            var checkFatigue = new CheckFatigue(moveStamina);
            var defaultTexts = data.GetDefaultDialogues();
            var angerTexts = data.GetEmotionDialogues(Emotion.Anger);
            
            Selector tree = new Selector("Pemabuk BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Intimate
                    new Sequence("Intimate Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Anger", characterTraits, new List<Node>
                        {
                            new TaskTalk(characterAI, defaultTexts),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Pull",new List<Node>
                            {
                                new CheckDirection(characterAI, isDifferentDir: true),
                                new TaskMoveToTarget(characterAI, data, isWalk: true),
                                new TaskPush(characterAI, characterAnim, pushForce, pushDelay),
                                new TaskTalk(characterAI, angerTexts),
                                new TaskMoveToOrigin(characterAI, data, isWalk: true)
                            }),
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