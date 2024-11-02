using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ATBMI
{
    [CreateAssetMenu(fileName = "QuestInfoList", menuName = "Data/Quest/Quest Info List", order = 1)]
    public class QuestInfoListSO : ScriptableObject
    {
        public List<QuestInfoSO> QuestInfoList = new();

        private void OnValidate()
        {
            #if UNITY_EDITOR
            QuestInfoList = QuestInfoList.OrderBy(x => x.QuestId).ToList();
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}
