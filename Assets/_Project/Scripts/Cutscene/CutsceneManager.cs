using UnityEngine;
using ATBMI.Core;

namespace ATBMI.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        #region Fields
        
        // Main
        public static CutsceneManager Instance;
        public bool IsCutscenePlaying { get; private set; }
        
        [Header("Reference")]
        [SerializeField] private Transform playerTransform;
        
        public Transform Player
        {
            get
            {
                if (playerTransform.CompareTag(GameTag.PLAYER_TAG))
                    return playerTransform;
                
                Debug.Log("player transform is not player!\n check the tag!");
                return null;
            }
        }
        
        #endregion

        #region Methods
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void OnCutscenePlay() => IsCutscenePlaying = true;
        public void OnCutsceneStop() => IsCutscenePlaying = false;
        
        #endregion
    }
}