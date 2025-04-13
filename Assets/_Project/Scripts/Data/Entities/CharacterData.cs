using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Entities/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [Serializable]
        private struct CharacterMoves
        {
            public string Type;
            public float Speed;
        }
        
        [Serializable]
        private struct CharacterDialogues
        {
            public string sceneName;
            public TextAsset dialogues;
        }
        
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private bool isIdling;
        [SerializeField] [HideIf("isIdling")] 
        private CharacterMoves[] moveSpeeds;

        [Header("Properties")]
        [SerializeField] private CharacterDialogues[] defaultDialogues;
        [Space]
        [SerializeField] private ItemList itemListSO;
        [SerializeField] private List<ItemDialogue> itemDialogues;
        
        // Stats
        public string CharacterName => characterName;
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
  
        // Properties
        public TextAsset GetDefaultDialogue(string scene = "null")
        {
            if (defaultDialogues.Length == 0)
            {
                Debug.LogError("default dialogues not set!");
                return null;
            }
            
            if (scene == "null")
                return defaultDialogues[0].dialogues;
                    
            foreach (var dialogue in defaultDialogues)
            {
                if (dialogue.sceneName == scene)
                    return dialogue.dialogues;
            }
            
            Debug.LogError("scene default dialogue not found!");
            return null;
        }
        
        public ItemList ItemList => itemListSO;
        public TextAsset GetItemDialogue(ItemData item)
        {
            var entry = itemDialogues.Find(d => d.item == item);
            return entry != null ? entry.dialogue : defaultDialogues[0].dialogues;
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
