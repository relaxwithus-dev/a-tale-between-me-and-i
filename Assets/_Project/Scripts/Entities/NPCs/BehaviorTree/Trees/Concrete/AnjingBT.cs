using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class AnjingBT : Trees
    {
        [Header("Attribute")] 
        [SerializeField] private float followTime;
        [SerializeField] private Transform centerPoint;
        [SerializeField] private float areaRadius;
        [SerializeField] private LayerMask layerMask;
        
        [Header("Reference")]
        [SerializeField] private CharacterInteract interact;
        
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Anjing BT", new List<Node>
            {
                new CheckInteracted(interact),
                new Sequence("Check Target In Area", new List<Node>
                {
                    new CheckTargetInZone(centerPoint, areaRadius, layerMask),
                    new Sequence("Follow", new List<Node>
                    {
                        new TaskFollow(characterAI, characterAI.Data, followTime),
                        new TaskIdle(characterAI)
                    })
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(centerPoint.position, areaRadius);
        }
    }
}