using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ATBMI.Enum;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewInteractionData", menuName = "Data/Interaction Data", order = 0)]
    public class InteractionData : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private CollectibleState interactionAction;
        [SerializeField] private TextAsset dialogueAsset;
        [SerializeField] private bool hasItem;
        [SerializeField] private GameObject itemObject;

        public CollectibleState InteractionAction => interactionAction;
        public TextAsset DialogueAsset => dialogueAsset;
        public bool HasItem => hasItem;
        public GameObject ItemObject => itemObject;


        #if UNITY_EDITOR
        [CustomEditor(typeof(InteractionData))]
        class InteractionDataEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                var data = (InteractionData)target;
                serializedObject.Update();

                if (data.hasItem)
                    DrawDefaultInspector();
                else 
                    DrawPropertiesExcluding(serializedObject,"itemObject");

                serializedObject.ApplyModifiedProperties();
            }
        }
        #endif
    }
}