using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Inventory
{
    public class InventoryBarCreator : MonoBehaviour
    {
        #region Global Fields

        [Header("General")]
        public GameObject inventoryPrefab;
        public Transform inventoryParent;
        public List<FlagBase> inventoryFlags;
        public IInventorySlot inventorySlot;

        public List<FlagBase> InventoryFlags => inventoryFlags;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            PlayerEvents.OnUpdateInventory += ModifyInventory;
        }

        private void OnDisable()
        {
            PlayerEvents.OnUpdateInventory -= ModifyInventory;
        }

        public void Start()
        {
            InitOnStart();
        }

        #endregion

        #region Methods

        // !- Initialize
        protected virtual void InitOnStart() { }

        // !- Core
        protected virtual void ModifyInventory(List<InventoryItem> inventoryList) {}

        public void CreateInventorySlot()
        {
            var slot = inventorySlot.CreateInventorySlot();
            inventoryFlags.Add(slot);
        }

        protected void RemoveInventorySlot(int index)
        {
            if (inventoryFlags[index] is MonoBehaviour mono)
                Destroy(mono.gameObject);

            inventoryFlags.RemoveAt(index);
        }

        #endregion
    }
}
