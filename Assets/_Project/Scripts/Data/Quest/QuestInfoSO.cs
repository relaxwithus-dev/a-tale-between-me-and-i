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
        public bool shoulBeFinishManually;

        [Header("Requirements")]
        public QuestInfoSO[] questPrerequisites;

        [Header("Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Reward")]
        public ItemData[] rewardItem;
    }
}
