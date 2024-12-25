using UnityEngine;
using ATBMI.Data;

namespace ATBMI
{
    public class CharacterAI : MonoBehaviour
    {
        #region Fields & Properties

        [Header("General")]
        [SerializeField] protected NPCData npcData;
        [SerializeField] private bool isFacingRight;

        public bool IsFacingRight => isFacingRight;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
