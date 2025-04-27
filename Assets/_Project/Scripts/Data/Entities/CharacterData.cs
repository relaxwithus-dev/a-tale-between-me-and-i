using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Entities.NPCs;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Entities/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        #region Struct

        [Serializable]
        private struct MoveStats
        {
            public string Type;
            public float Speed;
        }
        
        [Serializable]
        private struct Dialogues
        {
            public string sceneName;
            public TextAsset dialogues;
        }
        
        [Serializable]
        private struct EmotionDialogues
        {
            public Emotion emotion;
            public TextAsset[] textAssets;
        }
        
        #endregion

        private enum CharacterType { Normal, Emotion, Story }
        
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private string animationTagName;
        [SerializeField] [EnumToggleButtons] private CharacterType characterType;
        [SerializeField] private bool isIdling;
        [SerializeField] [HideIf("isIdling")] 
        private MoveStats[] moveSpeeds;
        
        [Header("Dialogue")]
        [SerializeField] private ItemList itemListSO;
        
        [SerializeField] private List<TextAsset> defaultDialogues;
        [SerializeField] private List<Dialogues> sceneDialogues;
        [SerializeField] [ShowIf("characterType", CharacterType.Emotion)]
        private List<EmotionDialogues> emotionDialogues;
        [SerializeField] [ShowIf("characterType", CharacterType.Story)]
        private List<TextAsset> storyDialogue;
        [SerializeField] private List<ItemDialogue> itemDialogues;
        
        // Stats
        public string CharacterName => characterName;
        public string AnimationTagName => animationTagName;
        public float GetSpeedByType(string type)
        {
            foreach (var speed in moveSpeeds)
            {
                if (speed.Type == type)
                    return speed.Speed;
            }
            
            Debug.LogError("move speeds not found!");
            return 0f;
        }
        
        // Dialogue
        public ItemList ItemList => itemListSO;
        public TextAsset[] GetDefaultDialogue() => defaultDialogues.ToArray();
        public TextAsset GetDefaultDialogueByScene(string scene = "null")
        {
            if (sceneDialogues.Count == 0)
            {
                Debug.LogError("default dialogues not set!");
                return null;
            }
            
            if (scene == "null")
                return sceneDialogues[0].dialogues;
                    
            foreach (var dialogue in sceneDialogues)
            {
                if (dialogue.sceneName == scene)
                    return dialogue.dialogues;
            }
            
            Debug.LogError("scene default dialogue not found!");
            return null;
        }
        
        public TextAsset[] GetEmotionDialogues(Emotion emotion)
        {
            var asset = emotionDialogues.Find(x => x.emotion == emotion);
            return asset.textAssets;
        }
        
        public TextAsset GetItemDialogue(ItemData item)
        {
            var entry = itemDialogues.Find(d => d.item == item);
            return entry != null ? entry.dialogue : sceneDialogues[0].dialogues;
        }
        
        
    #if UNITY_EDITOR
        // Automatically call the method when there is a changes in itemlistSO
        // NOTE: still need to assign the item-spesific dialogue on changes
        private void OnValidate()
        {
            if (itemListSO != null)
            {
                ItemListReferenceRegistry.RegisterCharacterData(this, itemListSO);
            }
            UpdateItemDialogues();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void UpdateItemDialogues()
        {
            if (itemListSO == null) return;

            // Ensure item dialogues match the item list
            itemDialogues = itemDialogues
                .Where(d => itemListSO.itemList.Contains(d.item))
                .ToList();

            foreach (var item in itemListSO.itemList)
            {
                if (itemDialogues.All(d => d.item != item))
                {
                    itemDialogues.Add(new ItemDialogue { item = item, dialogue = null });
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
    #endif
    }
}
