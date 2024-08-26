using System;
using UnityEditor;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class CharacterInteraction : Interaction
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private Exchange itemExchange;
        [SerializeField] private int itemTargetId;

        public Exchange ItemExchange => itemExchange;

        #endregion

        #region Methods

        public override void Interact(InteractManager manager)
        {
            base.Interact(manager);
            throw new NotImplementedException();
        }

        #endregion
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(CharacterInteraction))]
    class CharacterInteractionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var data = (CharacterInteraction)target;
            serializedObject.Update();

            if (data.ItemExchange == Exchange.Give)
                DrawDefaultInspector();
            else 
                DrawPropertiesExcluding(serializedObject,"itemTargetId");
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}