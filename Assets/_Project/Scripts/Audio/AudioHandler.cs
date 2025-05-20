using UnityEngine;

namespace ATBMI.Audio
{
    public class AudioHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")] 
        [SerializeField] private Musics musicName;
        [SerializeField] private float fadeDuration = 0.5f;
        
        #endregion

        #region Methods

        // Unity Callbacks
        private void OnEnable()
        {
            AudioEvent.OnFadeInAudio += FadeInAudio;
            AudioEvent.OnFadeOutAudio += FadeOutAudio;
        }
        
        private void OnDisable()
        {
            AudioEvent.OnFadeInAudio -= FadeInAudio;
            AudioEvent.OnFadeOutAudio -= FadeOutAudio;
        }
        
        private void Start()
        {
            var currentMusic = AudioManager.Instance.GetAudio(musicName);
            if (currentMusic.name == musicName.ToString()) return;
            FadeInAudio();
        }
        
        // Core
        private void FadeInAudio() => FadeAudio(isFadeIn: true);
        private void FadeOutAudio() => FadeAudio(isFadeIn: false);
        
        private void FadeAudio(bool isFadeIn)
        {
            var musics = AudioManager.Instance.GetAudio(musicName);
            
            StartCoroutine(isFadeIn
                ? musics.source.FadeIn(fadeDuration, musics.volume)
                : musics.source.FadeOut(fadeDuration));
        }
        
        #endregion
    }
}