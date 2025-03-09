using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Entities/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private float moveSpeed;

        [Space(20)]
        [Header("Default Dialogue/s")]
        [SerializeField] private TextAsset defaultDialogue; //TODO: add some default dialogue in different chapters, cange it to list

        [Header("Item-Specific Dialogue")]
        [SerializeField] private ItemList itemListSO;
        [SerializeField] private List<ItemDialogue> itemDialogues = new();

        // Getter
        public string CharacterName => characterName;
        public float MoveSpeed => moveSpeed;
        public TextAsset DefaultDialogue => defaultDialogue;
        public List<ItemDialogue> ItemDialogues => itemDialogues;
        public ItemList ItemList => itemListSO;

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
