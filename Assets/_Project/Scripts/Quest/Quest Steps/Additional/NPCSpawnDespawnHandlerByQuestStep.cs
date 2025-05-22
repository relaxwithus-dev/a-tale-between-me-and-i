using System;
using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Entities.NPCs;
using ATBMI.Gameplay.Event;
using ATBMI.Scene;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ATBMI
{
    [Serializable]
    public struct NpcSpawnData
    {
        [BoxGroup("Spawn NPC Settings")] public CharacterAI npcPrefab;
        [BoxGroup("Spawn NPC Settings")] public Vector2 spawnPoint;
        [BoxGroup("Spawn NPC Settings")] public SceneAsset spawnSceneName;
    }

    [Serializable]
    public struct NpcDespawnData
    {
        [BoxGroup("Despawn NPC Settings")] public CharacterData npcToDespawnReference;
        [BoxGroup("Despawn NPC Settings")] public SceneAsset despawnSceneName;
    }

    public class NPCSpawnDespawnHandlerByQuestStep : MonoBehaviour
    {
        [Header("Spawn Control")]
        [ToggleLeft]
        public bool spawnNPC;
        [ShowIf("spawnNPC")] public List<NpcSpawnData> spawnNPCData;

        [Header("Despawn Control")]
        [ToggleLeft]
        public bool despawnNPC;
        [ShowIf("despawnNPC")] public List<NpcDespawnData> despawnNPCData;

        private List<CharacterAI> currentSceneNPCs = new();
        private Dictionary<string, GameObject> spawnedNPCs = new(); // prefab => instance

        private Dictionary<string, bool> npcSpawnedStatus = new();
        private Dictionary<string, bool> npcDespawnedStatus = new();

        private void OnEnable()
        {
            QuestEvents.RegisterThisNPCToHandledByQuestStep += RegisterThisSceneNPCs;
            QuestEvents.UnregisterNPCsThatHandledByQuestStep += UnregisterThisSceneNPCs;

            GameEvents.OnChangeScene += OnChangeScene;
        }

        private void OnDisable()
        {
            QuestEvents.RegisterThisNPCToHandledByQuestStep -= RegisterThisSceneNPCs;
            QuestEvents.UnregisterNPCsThatHandledByQuestStep -= UnregisterThisSceneNPCs;

            GameEvents.OnChangeScene -= OnChangeScene;
        }

        private void RegisterThisSceneNPCs(CharacterAI npc)
        {
            currentSceneNPCs.Add(npc);
        }

        private void UnregisterThisSceneNPCs()
        {
            currentSceneNPCs.Clear();
        }

        private void OnChangeScene()
        {
            TrySpawnAndDespawn();
        }

        private void Start()
        {
            QuestEvents.InitiateRegisterNPCsEvent();

            TrySpawnAndDespawn();
        }

        private void TrySpawnAndDespawn()
        {
            if (spawnNPC)
            {
                foreach (var spawnData in spawnNPCData)
                {
                    if (IsCurrentScene(spawnData.spawnSceneName) &&
                        (!npcSpawnedStatus.TryGetValue(spawnData.npcPrefab.Data.CharacterName, out bool hasSpawned) || !hasSpawned))
                    {
                        Spawn(spawnData);
                        npcSpawnedStatus[spawnData.npcPrefab.Data.CharacterName] = true;
                    }
                }
            }

            if (despawnNPC)
            {
                foreach (var despawnData in despawnNPCData)
                {
                    if (IsCurrentScene(despawnData.despawnSceneName) &&
                        (!npcDespawnedStatus.TryGetValue(despawnData.npcToDespawnReference.CharacterName, out bool hasDespawned) || !hasDespawned))
                    {
                        Despawn(despawnData);
                        npcDespawnedStatus[despawnData.npcToDespawnReference.CharacterName] = true;
                    }
                }
            }
        }

        private bool IsCurrentScene(SceneAsset targetScene)
        {
            return targetScene == SceneNavigation.Instance.CurrentScene;
        }

        private void Spawn(NpcSpawnData data)
        {
            if (data.npcPrefab == null || spawnedNPCs.ContainsKey(data.npcPrefab.Data.CharacterName))
                return;

            spawnedNPCs[data.npcPrefab.Data.CharacterName] = Instantiate(data.npcPrefab.gameObject, data.spawnPoint, Quaternion.identity);

            Debug.Log("[NPCSpawnDespawnHandler] Spawned " + data.npcPrefab.name + " in scene " + data.spawnSceneName.name + " at " + data.spawnPoint);
        }

        private void Despawn(NpcDespawnData data)
        {
            if (data.npcToDespawnReference.CharacterName == null)
                return;

            foreach (var npc in currentSceneNPCs)
            {
                if (npc.gameObject.name == data.npcToDespawnReference.CharacterName)
                {
                    Debug.Log("[NPCSpawnDespawnHandler] Despawned " + npc.name + "in scene " + data.despawnSceneName.name + " at " + npc.transform.position);
                    Destroy(npc.gameObject);
                    return;
                }
            }

            Debug.LogWarning("[NPCSpawnDespawnHandler] NPC " + data.npcToDespawnReference.CharacterName + " not found in scene " + data.despawnSceneName.name);
        }
    }
}
