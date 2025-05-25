using UnityEngine;
using ATBMI.Scene;
using ATBMI.Quest;
using ATBMI.Cutscene;
using ATBMI.Inventory;
using ATBMI.Scene.Chapter;
using ATBMI.Gameplay.Event;

namespace ATBMI.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields
        
        [Header("Components")] 
        [SerializeField] private bool isGamePlaying;
        [SerializeField] private GameObject[] gameplayObjects;
        
        public bool IsGamePlaying => isGamePlaying;
        
        [Header("Reference")] 
        [SerializeField] private QuestManager questManager;
        [SerializeField] private CutsceneManager cutsceneManager;
        [SerializeField] private InventoryManager inventoryManager;
        
        #endregion

        #region Methods

        private void OnEnable()
        {
            GameEvents.OnGameStart += StartGame;
            GameEvents.OnGameExit += ExitGame;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= StartGame;
            GameEvents.OnGameExit -= ExitGame;
        }

        private void StartGame()
        {
            // Setup gameplay objects
            isGamePlaying = true;
            foreach (var gameplay in gameplayObjects)
            {
                gameplay.SetActive(true);
            }
        }
        
        private void ExitGame()
        {
            // Setup gameplay objects
            isGamePlaying = false;
            foreach (var gameplay in gameplayObjects)
            {
                gameplay.SetActive(false);
            }
            
            // Reset global progression
            questManager.ResetQuest();
            cutsceneManager.ResetCutscene();
            inventoryManager.ResetInventory();
            ChapterViewer.Instance.UpdateChapter("Prologue");
            SceneNavigation.Instance.ChapterName = Chapters.None;
        }
        
        #endregion
    }
}
