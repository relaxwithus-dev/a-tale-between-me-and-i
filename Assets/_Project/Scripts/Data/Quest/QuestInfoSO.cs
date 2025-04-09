using ATBMI.Data;
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
    }

    [System.Serializable]
    public struct QuestStepEntry
    {
        public string targetScene; //TODO: change to enum
        public GameObject questStepPrefab;
        [TextArea] public string stepStatusText;
    }
}
