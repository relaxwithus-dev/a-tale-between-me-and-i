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
        
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private bool isIdling;
        [SerializeField] [HideIf("isIdling")] 
        private CharacterMoves[] moveSpeeds;

        [Space(20)]
        [Header("Default Dialogue/s")]
        [SerializeField] private TextAsset defaultDialogue; //TODO: add some default dialogue in different chapters, cange it to list

        [Header("Item-Specific Dialogue")]
        [SerializeField] private ItemList itemListSO;
        [SerializeField] private List<ItemDialogue> itemDialogues = new();

        // General
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
        public TextAsset DefaultDialogue => defaultDialogue;
        public List<ItemDialogue> ItemDialogues => itemDialogues;
        public ItemList ItemList => itemListSO;

        #region Method
        public TextAsset GetItemDialogue(ItemData item)
        {
            var entry = itemDialogues.Find(d => d.item == item);
            return entry != null ? entry.dialogue : defaultDialogue;
        }
        #endregion

        #region OnValidate Item-Specific Dialogue
        // Automatically call the method when there is a changes in itemlistSO
        // NOTE: still need to assign the item-spesific dialogue on changes
#if UNITY_EDITOR
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
                if (!itemDialogues.Any(d => d.item == item))
                {
                    itemDialogues.Add(new ItemDialogue { item = item, dialogue = null });
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        #endregion
    }
}
