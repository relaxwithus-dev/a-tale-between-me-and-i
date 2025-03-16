using UnityEngine;

namespace ATBMI.Inventory
{
    public class InteractSlot : IInventorySlot
    {
        private readonly GameObject interactPrefab;
        private readonly Transform parentTransform;

        public InteractSlot(GameObject prefab, Transform parent)
        {
            this.interactPrefab = prefab;
            this.parentTransform = parent;
        }

        public FlagBase CreateInventorySlot()
        {
            var slowObj = Object.Instantiate(interactPrefab, parentTransform, worldPositionStays: false);

            return slowObj.GetComponent<InteractFlag>();
        }
    }
}
