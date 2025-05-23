using System.Linq;
using ATBMI.Data;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ATBMI
{
    [CreateAssetMenu(fileName = "NewQuestInfo", menuName = "Data/Quest/Quest Info", order = 0)]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public int QuestId { get; private set; }
        [Header("General")]
        public string displayName;
        public bool shouldBeFinishManually;

        [Header("Requirements")]
        public QuestInfoSO[] questPrerequisites;

        [Header("Steps")]
        public QuestStepEntry[] questSteps;

        [Header("Reward")]
        public ItemData[] rewardItem;

#if UNITY_EDITOR
        [Button("Add to QuestInfoListSO", ButtonSizes.Large)]
        private void AddToQuestList()
        {
            string[] guids = AssetDatabase.FindAssets("t:QuestInfoListSO");

            if (guids.Length == 0)
            {
                Debug.LogWarning("No QuestInfoListSO asset found in the project.");
                return;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            QuestInfoListSO questList = AssetDatabase.LoadAssetAtPath<QuestInfoListSO>(path);

            if (questList.QuestInfoList.Contains(this))
            {
                Debug.Log(name + " is already in the QuestInfoList.");
                return;
            }

            questList.QuestInfoList.Add(this);
            questList.QuestInfoList = questList.QuestInfoList.OrderBy(q => q.QuestId).ToList();

            EditorUtility.SetDirty(questList);
            AssetDatabase.SaveAssets();

            Debug.Log(name + " added into the QuestInfoList.");
        }
#endif

    }

    [System.Serializable]
    public struct QuestStepEntry
    {
        public string targetScene; //TODO: change to enum
        public GameObject questStepPrefab;
        [TextArea] public string stepStatusText;
    }
}
