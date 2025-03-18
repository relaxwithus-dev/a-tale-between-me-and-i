#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using ATBMI.Data;

namespace ATBMI
{
    [InitializeOnLoad]
    public static class ItemListReferenceRegistry
    {
        private static Dictionary<ItemList, List<CharacterData>> referenceCache = new();

        static ItemListReferenceRegistry()
        {
            RefreshCache();
        }

        public static void RegisterCharacterData(CharacterData characterData, ItemList itemList)
        {
            if (!referenceCache.ContainsKey(itemList))
            {
                referenceCache[itemList] = new List<CharacterData>();
            }

            if (!referenceCache[itemList].Contains(characterData))
            {
                referenceCache[itemList].Add(characterData);
            }
        }

        public static List<CharacterData> GetCharacterData(ItemList itemList)
        {
            return referenceCache.TryGetValue(itemList, out var characterDataList) ? characterDataList : new List<CharacterData>();
        }

        public static void RefreshCache()
        {
            referenceCache.Clear();
            string[] characterDataGUIDs = AssetDatabase.FindAssets("t:CharacterData");

            foreach (string guid in characterDataGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(path);

                if (characterData != null && characterData.ItemList != null)
                {
                    RegisterCharacterData(characterData, characterData.ItemList);
                }
            }
        }
    }
}
#endif
