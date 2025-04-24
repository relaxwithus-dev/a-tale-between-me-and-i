using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class KatingBT : Trees
    {
        [Header("Attribute")] 
        [SerializeField] private float moveDelay;
        [SerializeField] private Transform[] wayPoints;
        
        [Header("Reference")]
        [SerializeField] private CharacterInteract interact;
        [SerializeField] private CharacterManager manager;
        
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Kating BT", new List<Node>
            {
                new CheckInteracted(interact),
                new Sequence("Patrol", new List<Node>
                {
                    new CheckFatigue(manager),
                    new TaskPatrol(characterAI, manager, characterAI.Data, wayPoints, moveDelay)
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}