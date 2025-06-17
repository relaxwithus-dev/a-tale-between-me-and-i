using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ATBMI.Gameplay.Event;
using ATBMI.Interaction;
using System;
using Sirenix.OdinInspector;
using ATBMI.Scene;
using ATBMI.Data;

namespace ATBMI.Quest
{
    [Serializable]
    public struct ItemSpawnData
    {
        [BoxGroup("Spawn Item Settings")] public ItemInteract itemPrefab;
        [BoxGroup("Spawn Item Settings")] public Vector2 spawnPoint;
        [BoxGroup("Spawn Item Settings")] public SceneAsset spawnSceneName;
    }

    [Serializable]
    public struct ItemDespawnData
    {
        [BoxGroup("Despawn Item Settings")] public ItemData itemToDespawnReference;
        [BoxGroup("Despawn Item Settings")] public SceneAsset despawnSceneName;
    }

    public class GetItemsStep : QuestStep
    {
        [Header("Item Collection List")]
        [SerializeField] private List<int> itemIds;

        [Space(20)]
        [Header("Spawn Control")]
        [ToggleLeft]
        public bool spawnItem;
        [ShowIf("spawnItem")] public List<ItemSpawnData> spawnItemData;

        [Header("Despawn Control")]
        [ToggleLeft]
        public bool despawnItem;
        [ShowIf("despawnItem")] public List<ItemDespawnData> despawnItemData;

        private List<ItemInteract> currentSceneItems = new();
        private Dictionary<int, GameObject> spawnedItems = new(); // prefab => instance
        private Dictionary<int, bool> itemSpawnedStatus = new();
        private Dictionary<int, bool> itemDespawnedStatus = new();

        private Dictionary<int, bool> itemStates = new();

        private void Awake()
        {
            foreach (int id in itemIds)
            {
                itemStates[id] = false; // All items start as not collected
            }
        }

        private void OnEnable()
        {
            QuestEvents.RegisterThisItemToHandledByQuestStep += RegisterThisSceneItems;
            QuestEvents.UnregisterItemsThatHandledByQuestStep += UnregisterThisSceneItems;

            GameEvents.OnChangeScene += OnChangeScene;

            QuestEvents.GetItemQuestStep += GetItem;
        }

        private void OnDisable()
        {
            QuestEvents.GetItemQuestStep -= GetItem;
        }

        private void Start()
        {
            TrySpawnAndDespawn();
        }

        private void RegisterThisSceneItems(ItemInteract item)
        {
            currentSceneItems.Add(item);
        }

        private void UnregisterThisSceneItems()
        {
            currentSceneItems.Clear();
        }

        private void OnChangeScene()
        {
            TrySpawnAndDespawn();
        }

        private void TrySpawnAndDespawn()
        {
            if (spawnItem)
            {
                foreach (var spawnData in spawnItemData)
                {
                    if (IsCurrentScene(spawnData.spawnSceneName) &&
                        (!itemSpawnedStatus.TryGetValue(spawnData.itemPrefab.Data.ItemId, out bool hasSpawned) || !hasSpawned))
                    {
                        Spawn(spawnData);
                        itemSpawnedStatus[spawnData.itemPrefab.Data.ItemId] = true;
                    }
                }
            }

            if (despawnItem)
            {
                foreach (var despawnData in despawnItemData)
                {
                    if (IsCurrentScene(despawnData.despawnSceneName) &&
                        (!itemDespawnedStatus.TryGetValue(despawnData.itemToDespawnReference.ItemId, out bool hasDespawned) || !hasDespawned))
                    {
                        Despawn(despawnData);
                        itemDespawnedStatus[despawnData.itemToDespawnReference.ItemId] = true;
                    }
                }
            }
        }

        private bool IsCurrentScene(SceneAsset targetScene)
        {
            return targetScene == SceneNavigation.Instance.CurrentScene;
        }

        private void Spawn(ItemSpawnData data)
        {
            if (data.itemPrefab == null || spawnedItems.ContainsKey(data.itemPrefab.Data.ItemId))
                return;

            spawnedItems[data.itemPrefab.Data.ItemId] = Instantiate(data.itemPrefab.gameObject, data.spawnPoint, Quaternion.identity);

            Debug.Log("[ItemSpawnDespawnHandler] Spawned " + data.itemPrefab.name + " in scene " + data.spawnSceneName.name + " at " + data.spawnPoint);
        }

        private void Despawn(ItemDespawnData data)
        {
            if (data.itemToDespawnReference.ItemId == -1)
                return;

            foreach (var item in currentSceneItems)
            {
                if (item.Data.ItemId == data.itemToDespawnReference.ItemId)
                {
                    Debug.Log("[ItemSpawnDespawnHandler] Despawned " + item.name + "in scene " + data.despawnSceneName.name + " at " + item.transform.position);
                    Destroy(item.gameObject);
                    return;
                }
            }

            Debug.LogWarning("[ItemSpawnDespawnHandler] tem " + data.itemToDespawnReference.ItemId + " not found in scene " + data.despawnSceneName.name);
        }

        private void GetItem(int id)
        {
            if (itemStates.ContainsKey(id))
            {
                itemStates[id] = true;

                if (IsAllItemsCollected())
                {
                    UpdateStepState(QuestStepStatusEnum.Finished);
                    FinishQuestStep();
                }
                else
                {
                    UpdateStepState(QuestStepStatusEnum.In_Progress);
                }
            }
        }

        private bool IsAllItemsCollected()
        {
            return itemStates.Values.All(status => status == true);
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            string state = SerializeItemStates();
            // Debug.Log(state);
            ChangeState(state, stepStatus);
        }

        // Serialize the dictionary into a string. e.g "1:true,2:false,3:true"
        private string SerializeItemStates()
        {
            return string.Join(",", itemStates.Select(pair => $"{pair.Key}:{pair.Value}"));
        }

        // Deserialize the string back to the dictionary
        protected override void SetQuestStepState(string state)
        {
            itemStates = state
                .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(pair => pair.Split(':'))
                .ToDictionary(
                    parts => int.Parse(parts[0]),
                    parts => bool.Parse(parts[1])
                );

            UpdateStepState(IsAllItemsCollected() ? QuestStepStatusEnum.Finished : QuestStepStatusEnum.In_Progress);
        }
    }
}
