using System.Collections;
using UnityEngine;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene.Chapter
{
    public class ChapterInitiator : SceneLoader
    {
        [Header("Initiator")] 
        [SerializeField] private string playerName;
        [SerializeField] private float animateDuration = 3f;
        [SerializeField] private ChapterViewer.Chapters chapterName;
        [SerializeField] private FadeController chapterFader;
        
        // Reference
        private ChapterViewer _chapterViewer;
        
        protected override IEnumerator AnimateSceneFade()
        {
            _chapterViewer = ChapterViewer.Instance;
            if (!_chapterViewer.IsInitializedChapter && chapterName ==  _chapterViewer.Chapter) 
                yield return AnimateChapterIntro();
            
            yield return base.AnimateSceneFade();
        }
        
        private IEnumerator AnimateChapterIntro()
        {
            yield return new WaitForSeconds(chapterFader.FadeDuration);
            chapterFader.gameObject.SetActive(true);
            chapterFader.DoFade(0f, 0f);
            chapterFader.FadeOut();
            
            yield return new WaitForSeconds(chapterFader.FadeDuration * animateDuration);
            chapterFader.FadeIn();
            player.InitPlayer(playerName);
        }
    }
}