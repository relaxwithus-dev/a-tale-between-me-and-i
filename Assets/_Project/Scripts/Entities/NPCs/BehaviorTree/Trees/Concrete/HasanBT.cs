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
           Selector moveTree = new Selector("Move Trere", new List<Node>
           {
               new Sequence("Move", new List<Node>
               {
                   new CheckTargetAvailable(targetPoints[0]),
                   new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, targetPoints[0]),
                   new TaskIdle(characterAI)
               }),
               new TaskIdle(characterAI)
           });
           
           // Patrol 
           Selector patrolTree = new Selector("Patrol Tree", new List<Node>
           {
               new Sequence("Patrol", new List<Node>
               {
                   new CheckIsFatigue(characterManager),
                   new TaskPatrol(characterAI, characterManager, characterAI.Data, targetPoints)
               }),
               new TaskIdle(characterAI)
           });
           
           
           return patrolTree;
        }
    }
}