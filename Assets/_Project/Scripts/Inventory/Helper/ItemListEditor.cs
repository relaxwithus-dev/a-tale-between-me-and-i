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
            // Draw the default inspector (so the regular fields show up)
            DrawDefaultInspector();

            // Get a reference to the target object (ItemList ScriptableObject)
            ItemList itemList = (ItemList)target;

            // Add a button that will trigger the SortItemList() method
            if (GUILayout.Button("Sort Item List By Item ID"))
            {
                // Call the SortItemList method when the button is pressed
                itemList.SortItemList();

                // Mark the object as dirty to save changes
                EditorUtility.SetDirty(itemList);
            }
        }
    }
}
