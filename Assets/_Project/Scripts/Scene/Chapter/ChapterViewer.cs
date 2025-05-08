using System;
using UnityEngine;

namespace ATBMI.Scene.Chapter
{
    public class ChapterViewer : MonoBehaviour
    {
        public enum Chapters { Prologue, Chapter_1, Chapter_2, None }

        [Header("Attribute")] 
        [SerializeField] private bool isInitializedChapter;
        [SerializeField] private Chapters chapter;
        
        public bool IsInitializedChapter => isInitializedChapter;
        public Chapters Chapter => chapter;
        
        public static ChapterViewer Instance;

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
            isInitializedChapter = false;
            chapter = Chapters.Prologue;
        }
        
        public void UpdateChapter(string chapterName)
        {
            var chapterTarget = GetChapters(chapterName);
            if (chapterTarget == Chapters.None) return;
            chapter = chapterTarget;
        }
        
        private Chapters GetChapters(string chapter)
        {
            if (System.Enum.TryParse<Chapters>(chapter, out var parsedChapter))
            {
                var allActions = System.Enum.GetValues(typeof(Chapters));
                foreach (Chapters enumAction in allActions)
                {
                    if (enumAction == parsedChapter)
                    {
                        return enumAction;
                    }
                }
            }
            
            return Chapters.None;
        }
    }
}