using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class HasanBT : EmoTrees
    {
        [Header("Properties")]
        [SerializeField] private Transform[] targetPoints;
        [SerializeField] private CharacterManager characterManager;
        
        protected override Node SetupTree()
        {
           // Move Target Behavior
           Selector moveTree = new Selector("Move Tree", new List<Node>
           {
               new Sequence("Move", new List<Node>
               {
                   new CheckReachTarget(characterAI, targetPoints[0]),
                   new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, targetPoints[0]),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           Selector moveAndBackTree = new Selector("Move and Back Tree", new List<Node>
           {
               new Sequence("Move and Back", new List<Node>
               {
                   new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                   new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true),
                   new TaskDialogue(characterAI, "Hi"),
                   new TaskMoveToOrigin(characterAI, characterAI.Data, isWalk: true),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           // Patrol 
           Selector patrolTree = new Selector("Patrol Tree", new List<Node>
           {
               new Sequence("Patrol", new List<Node>
               {
                   new CheckFatigue(characterManager),
                   new TaskPatrol(characterAI, characterManager, characterAI.Data, targetPoints)
               }),
               new TaskIdle(characterAI)
           });
           
           // Run Away
           Selector runAwayTree = new Selector("Run Away Tree", new List<Node>
           {
               new Sequence("Run Away", new List<Node>
               {
                   new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                   new TaskRunAway(characterAI, characterAI.Data, 8f),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           // Follow
           Selector followTree = new Selector("Follow Tree", new List<Node>
           {
               new Sequence("Follow", new List<Node>
               {
                   new CheckTargetInZone(centerPoint, zoneDetails[2].Radius, layerMask),
                   new TaskFollow(characterAI, characterAI.Data, 6f),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           // Push
           Selector pushTree = new Selector("Push Tree", new List<Node>
           {
               new Sequence("Push", new List<Node>
               {
                   new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                   new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true),
                   new TaskPush(characterAI, force: 6f, delay: 0.15f),
                   new TaskMoveToOrigin(characterAI, characterAI.Data, isWalk: true),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           // Push
           Selector pullTree = new Selector("Pull Tree", new List<Node>
           {
               new Sequence("Pull", new List<Node>
               {
                   new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                   new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true),
                   new TaskPull(characterAI, force: 2f, delay: 0.15f),
                   new TaskMoveToOrigin(characterAI, characterAI.Data, isWalk: true),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           // Jump Back
           Selector jumpTree = new Selector("Jump Tree", new List<Node>
           {
               new Sequence("Jump", new List<Node>
               {
                   new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                   new CheckSameDirection(characterAI),
                   new TaskJumpBack(characterAI, 0.3f, 0.15f),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           return followTree;
        }
    }
}