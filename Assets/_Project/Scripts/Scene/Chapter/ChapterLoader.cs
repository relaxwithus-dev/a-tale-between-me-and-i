using UnityEngine;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene.Chapter
{
    public class ChapterLoader : MonoBehaviour
    {
        [Header("Loader")] 
        [SerializeField] private string playerName;
        [SerializeField] private ChapterViewer.Chapters chapterName;
        
        // Reference
        private FadeController _fader;
        private PlayerController _player;
        private ChapterViewer _chapterViewer;

        private void Awake()
        {
            _fader = FindObjectOfType<FadeController>();
            _player = FindObjectOfType<PlayerController>();
            _chapterViewer = FindObjectOfType<ChapterViewer>();
        }
        
        // TODO: Adjust animasi fade-nama chapter-fade
        private void OnEnable()
        {
            // Validate
            if (chapterName != _chapterViewer.Chapter) 
                return;
            
            _fader.gameObject.SetActive(true);
            _player.InitPlayer(playerName);
            _fader.FadeIn();
        }
    }
}