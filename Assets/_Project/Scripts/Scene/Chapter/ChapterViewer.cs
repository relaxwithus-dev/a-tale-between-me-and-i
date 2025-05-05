using UnityEngine;
using UnityEngine.Serialization;

namespace ATBMI.Scene.Chapter
{
    public class ChapterViewer : MonoBehaviour
    {
        public enum Chapters { Prologue, Chapter_1, Chapter_2, None }
        
        [Header("Attribute")]
        [SerializeField] private Chapters chapter;
        public Chapters Chapter => chapter;
        
        private void Start()
        {
            chapter = Chapters.Prologue;
        }
        
        // TODO: Crosscheck cara designer pindah Chapter
        public void UpdateChapter(string chapterName)
        {
            var chapterTarget = GetChapters(chapterName);
            if (chapterTarget == Chapters.None) return;
            chapter = chapterTarget;
        }
        
        private Chapters GetChapters(string action)
        {
            if (System.Enum.TryParse<Chapters>(action, out var parsedAction))
            {
                var allActions = System.Enum.GetValues(typeof(Chapters));
                foreach (Chapters enumAction in allActions)
                {
                    if (enumAction == parsedAction)
                    {
                        return enumAction;
                    }
                }
            }

            return Chapters.None;
        }
    }
}