using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class CleaningBT : Trees
    {
        [Header("Attribute")] 
        [SerializeField] private float moveStamina;
        [SerializeField] private float moveDelay;
        [SerializeField] private Transform[] wayPoints;
        
        [Header("Reference")]
        [SerializeField] private CharacterInteract interact;
        
        protected override Node SetupTree()
        {
            CheckFatigue checkFatigue = new CheckFatigue(moveStamina);
            Selector tree = new Selector("Cleaning BT", new List<Node>
            {
                new CheckInteracted(interact),
                new Sequence("Patrol", new List<Node>
                {
                    checkFatigue,
                    new TaskPatrol(characterAI, checkFatigue, wayPoints, moveDelay)
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}