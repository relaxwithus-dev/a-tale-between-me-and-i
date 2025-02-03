using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class EmoTrees : Trees
    {
        [Header("Zone")] 
        [SerializeField] protected Transform centerPoint;
        [SerializeField] protected ZoneDetail[] zoneDetails;
        [SerializeField] protected LayerMask layerMask;
        
        public ZoneDetail[] ZoneDetails => zoneDetails;
        
        protected override Node SetupTree()
        {
            throw new System.NotImplementedException();
        }
    }
}