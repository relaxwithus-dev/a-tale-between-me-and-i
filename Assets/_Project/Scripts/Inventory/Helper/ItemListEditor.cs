using UnityEngine;
using UnityEditor;

namespace ATBMI.Inventory
{
    [CustomEditor(typeof(SO_ItemList))]
    public class ItemListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector (so the regular fields show up)
            DrawDefaultInspector();

            // Get a reference to the target object (ItemList ScriptableObject)
            SO_ItemList itemList = (SO_ItemList)target;

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
