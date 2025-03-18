#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using ATBMI.Data;

namespace ATBMI
{
    public class ItemListPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                ItemList itemList = AssetDatabase.LoadAssetAtPath<ItemList>(assetPath);
                if (itemList != null)
                {
                    UpdateCharacterData(itemList);
                }
            }
        }

        private static void UpdateCharacterData(ItemList updatedItemList)
        {
            var characterDataList = ItemListReferenceRegistry.GetCharacterData(updatedItemList);

            foreach (var characterData in characterDataList)
            {
                characterData.UpdateItemDialogues();
                EditorUtility.SetDirty(characterData);
            }

            AssetDatabase.SaveAssets();
        }
    }
}
#endif