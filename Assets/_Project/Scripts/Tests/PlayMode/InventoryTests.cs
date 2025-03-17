using System.Collections;
using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Inventory;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ATBMI
{
    public class InventoryTests
    {
        private InventoryManager inventoryManager;
        // private InventoryCreator inventoryCreator;
        private ItemList testItemList;
        private GameObject inventoryManagerGameObject;
        // private GameObject inventoryGameObject;
        // private GameObject inventoryPrefab;
        // private Transform inventoryParent;

        [SetUp]
        public void SetUp()
        {
            // Create InventoryManager GameObject
            inventoryManagerGameObject = new GameObject("TestInventoryManager");
            inventoryManager = inventoryManagerGameObject.AddComponent<InventoryManager>();

            // inventoryGameObject = new GameObject("TestInventoryCreator");
            // inventoryCreator = inventoryGameObject.AddComponent<InventoryCreator>();

            // inventoryParent = new GameObject("TestParent").transform;
            // inventoryCreator.inventoryParent = inventoryParent;

            // inventoryPrefab = new GameObject("TestPrefab");
            // inventoryPrefab.AddComponent<InventoryFlag>(); // Ensure it has InventoryFlag
            // inventoryCreator.inventoryPrefab = inventoryPrefab;

            // Mock the ItemList ScriptableObject BEFORE Awake() is called
            testItemList = ScriptableObject.CreateInstance<ItemList>();
            testItemList.itemList = new List<ItemData>();

            // Create a test item (No need for reflection now)
            var testItem = ScriptableObject.CreateInstance<ItemData>();
            testItem.itemId = 123;
            testItem.itemName = "Test Item";
            testItem.itemDescription = "A test description";
            testItem.itemSprite = null;

            testItemList.itemList.Add(testItem);

            // Assign `itemList` before calling Awake()
            inventoryManager.itemList = testItemList;

            // Call Start() manually after everything is set to populate item data into dictionary
            inventoryManager.Start();

            // Force call InitOnStart()
            // inventoryCreator.Start();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(inventoryManagerGameObject);
            Object.DestroyImmediate(testItemList);
            // Object.DestroyImmediate(inventoryGameObject);
        }

        // [UnityTest]
        // public IEnumerator AddItemToInventory()
        // {
        //     int testItemId = 123;

        //     Debug.Log("Test item will added into the inventory with an item id of " + testItemId);

        //     // Ensure inventory starts empty
        //     if (inventoryManager.InventoryList.Count == 0)
        //         Debug.Log("Inventory is empty before adding an item.");
        //     else
        //         Assert.Fail("Inventory is not empty at test start.");

        //     // Act: Add item
        //     inventoryManager.AddItemToInventory(testItemId);
        //     yield return null;

        //     // Assert: Ensure item was added
        //     if (inventoryManager.InventoryList.Count >= 1)
        //         Debug.Log("Item was successfully added to inventory.");
        //     else
        //         Assert.Fail("Item was not added to inventory.");

        //     // Assert: Ensure correct item was added
        //     if (inventoryManager.InventoryList[0].ItemId == testItemId)
        //         Debug.Log("Correct item ID stored in inventory with an item id of " + testItemId);
        //     else
        //         Assert.Fail("Incorrect item ID in inventory.");
        // }

        [Test]
        public void PopulateItemDict()
        {
            // Act: Call PopulateItemDict()
            inventoryManager.PopulateItemDict();

            // Assert: Ensure dictionary is populated correctly
            Assert.IsTrue(inventoryManager.itemDatasDict.ContainsKey(123), "Dictionary does not contain the expected item ID.");
            Assert.AreEqual("Test Item", inventoryManager.itemDatasDict[123].ItemName, "Item name in dictionary does not match expected value.");
        }

        [UnityTest]
        public IEnumerator AddItemToInventory()
        {
            int testItemId = 123;

            // Act: Add item
            inventoryManager.AddItemToInventory(testItemId);
            yield return null;

            // Assert: Ensure item was added
            Assert.AreEqual(1, inventoryManager.InventoryList.Count, "Item was not added to inventory.");
        }

        [Test]
        public void GetItemData()
        {
            int testItemId = 123;

            // Act: Retrieve item data
            ItemData retrievedItem = inventoryManager.GetItemData(testItemId);

            // Assert: Check if the correct item is returned
            Assert.IsNotNull(retrievedItem, "Retrieved item should not be null.");
            Assert.AreEqual(testItemId, retrievedItem.ItemId, "Retrieved item ID does not match.");
            Assert.AreEqual("Test Item", retrievedItem.ItemName, "Retrieved item name does not match.");
        }

        [UnityTest]
        public IEnumerator RemoveItemFromInventory()
        {
            int testItemId = 123;

            // Act: Ensure item was added first before removing it
            inventoryManager.AddItemToInventory(testItemId);
            yield return null;

            // Assert: Check if the item is literally inside the inventorylist
            Assert.AreEqual(1, inventoryManager.InventoryList.Count, "Item should exist before deletion.");

            // Act: Remove item
            inventoryManager.RemoveItemFromInventory(testItemId);
            yield return null;

            // Assert: Check if the item is removed
            Assert.AreEqual(0, inventoryManager.InventoryList.Count, "Item was not removed from inventory.");
        }

        // [UnityTest]
        // public IEnumerator CreateInventorySlot_WhenItemAdded()
        // {
        //     int initialSlots = inventoryBarCreator.InventoryFlags.Count;

        //     // 🔹 Add an item to inventory (Triggers ModifyInventory)
        //     inventoryManager.AddItemToInventory(123);
        //     yield return null;

        //     // 🔹 Ensure a new inventory slot was created
        //     Assert.AreEqual(initialSlots + 1, inventoryBarCreator.InventoryFlags.Count, "Inventory slot was not created.");
        // }

        // [UnityTest]
        // public IEnumerator RemoveInventorySlot()
        // {
        //     // 🔹 Add an item first to ensure there's something to remove
        //     inventoryManager.AddItemToInventory(123);
        //     yield return null;

        //     int initialSlots = inventoryCreator.InventoryFlags.Count;

        //     // 🔹 Remove the item (Triggers ModifyInventory)
        //     inventoryManager.RemoveItemFromInventory(123);
        //     yield return null;

        //     // 🔹 Ensure a slot was removed
        //     Assert.AreEqual(initialSlots - 1, inventoryCreator.InventoryFlags.Count, "Inventory slot was not removed.");
        // }
    }
}
