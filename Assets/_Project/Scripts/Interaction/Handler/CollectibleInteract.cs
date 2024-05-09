using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public class CollectibleInteract : BaseInteract
    {
        #region Internal Fields

        [SerializeField] private CollectibleState collectiblesState;
        [SerializeField] private int targetId;

        public CollectibleState CollectibleState => collectiblesState;

        #endregion

        #region Methods

        // TODO: Drop logic untuk interaksi collectible item disini!

        public override void Interact()
        {
            base.Interact();
            Debug.Log("koncay wapwap collectible ya!");
        }

        public override void InteractCollectible(BaseInteract target)
        {
            base.InteractCollectible(target);
            switch (CollectibleState)
            {
                case CollectibleState.Receive:
                    break;
                case CollectibleState.Assign:
                    if (target.InteractId == targetId)
                        Debug.Log($"wah benda {target.name} ini sangat bagus!");
                    else
                        Debug.Log($"sepertinya benda {target.name} tidak cocok");
                    break;
                case CollectibleState.None:
                    break;
            }
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
