using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Core;
using ATBMI.Scene.Chapter;

namespace ATBMI.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct CutsceneStatusDetail
        {
            public Chapters chapters;
            public List<CutsceneStatus> status;
        }
        
        #endregion
        
        #region Fields & Properties
        
        [Header("Component")]
        [SerializeField] [Searchable] private List<CutsceneStatusDetail> statusList;
        [SerializeField] private Transform playerTransform;
        
        private int _currentIndex;
        private Chapters _currentChapter;
        
        public bool IsCutscenePlaying { get; private set; }
        public CutsceneKeys CurrentKeys { get; private set; }
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
        
        public static CutsceneManager Instance;
        
        // Event
        public static event Action OnCutsceneEnd;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        private void Start()
        {
            InitCutscene(Chapters.Prologue);
        }
        
        // Core
        public void InitCutscene(Chapters chapter)
        {
            _currentIndex = 0;
            _currentChapter = chapter;
            CurrentKeys = GetStatusDetail(chapter).status[_currentIndex].key;
        }
        
        private void UpdateCutscenes()
        {
            var status = GetStatusDetail(_currentChapter);
            if (status.status == null || _currentIndex >= status.status.Count)
                return;
            
            status.status[_currentIndex].isFinish = true;
            _currentIndex++;
            CurrentKeys = _currentIndex < status.status.Count ? 
                status.status[_currentIndex].key : null;
        }
        
        private CutsceneStatusDetail GetStatusDetail(Chapters chapter)
        {
            return statusList.FirstOrDefault(s => s.chapters == chapter);
        }
        
        public void EnterCutscene() => IsCutscenePlaying = true;
        public void ExitCutscene()
        {
            UpdateCutscenes();
            OnCutsceneEnd?.Invoke();
            IsCutscenePlaying = false;
        }
        public void ResetCutscene()
        {
            foreach (var status in statusList)
            {
                foreach (var key in status.status)
                {
                    key.isFinish = false;
                }
            }
        }
        
        #endregion
    }
}