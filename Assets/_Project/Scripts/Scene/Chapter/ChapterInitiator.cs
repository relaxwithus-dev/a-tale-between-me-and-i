using System.Collections;
using UnityEngine;
using ATBMI.Cutscene;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene.Chapter
{
    public class ChapterInitiator : SceneLoader
    {
        #region Fields

        [Header("Initiator")] 
        [SerializeField] private string playerName;
        [SerializeField] private float animateDuration = 3f;
        [SerializeField] private Chapters chapterName;
        [SerializeField] private FadeController chapterFader;
        
        // Reference
        private ChapterViewer _chapterViewer;
        
        #endregion

        #region Methods
        
        protected override IEnumerator AnimateSceneFade()
        {
            var currChapter = SceneNavigation.Instance.ChapterName;
            if (currChapter != chapterName)
            {
                _chapterViewer = ChapterViewer.Instance;
                SceneNavigation.Instance.ChapterName = chapterName;
                if (!_chapterViewer.IsInitializedChapter && chapterName ==  _chapterViewer.Chapter) 
                    yield return AnimateChapterIntro();
            }
            
            yield return base.AnimateSceneFade();
        }
        
        private IEnumerator AnimateChapterIntro()
        {
            yield return new WaitForSeconds(chapterFader.FadeDuration);
            chapterFader.gameObject.SetActive(true);
            chapterFader.DoFade(0f, 0f);
            chapterFader.FadeOut();
            
            yield return new WaitForSeconds(chapterFader.FadeDuration * animateDuration);
            CutsceneManager.Instance.InitCutscene(chapterName);
            player.InitPlayerStats(playerName);
            chapterFader.FadeIn();
        }
        
        #endregion
    }
}