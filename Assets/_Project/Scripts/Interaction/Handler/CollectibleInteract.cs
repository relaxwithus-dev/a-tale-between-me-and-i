using System;
using System.Collections.Generic;
using ATBMI.Enum;
using UnityEditor;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class CollectibleInteract : InteractionBase
    {
        #region Fields & Property
        
        [Header("Data")]
        [SerializeField] private int collectiblesId;
        [SerializeField] private TextAsset dialogueAsset;
        [SerializeField] private CollectibleState collectiblesState;
        [SerializeField] private int targetId;

        public CollectibleState CollectibleState => collectiblesState;

        #endregion

        #region Methods

        public override void InteractCollectible(CharacterInteract target)
        {
            base.InteractCollectible(target);
            if (target.CharacterId != targetId) return;
            // TODO: Drop method for dialogue here
            Debug.Log($"Collectible interacted on {target.name}");
        }

        #endregion
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(CollectibleInteract))]
    class InteractionDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var data = (CollectibleInteract)target;
            serializedObject.Update();

            if (data.CollectibleState == CollectibleState.Assign)
                DrawDefaultInspector();
            else 
                DrawPropertiesExcluding(serializedObject,"targetId");

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
