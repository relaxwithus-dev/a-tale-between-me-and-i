using UnityEngine;
using UnityEditor;
using ATBMI.Data;

namespace ATBMI.Inventory
{
    [CustomEditor(typeof(ItemList))]
    public class ItemListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ItemList itemList = (ItemList)target;
            if (GUILayout.Button("Sort Item List By Item ID"))
            {
                // Short item on list
                itemList.SortItemList();
                EditorUtility.SetDirty(itemList);
            }
        }
    }
}
