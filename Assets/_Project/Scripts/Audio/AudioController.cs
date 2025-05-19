using System;
using UnityEngine;

namespace ATBMI.Audio
{
    public class AudioController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private Musics musicName;
        
        // Event
        public static event Action<bool, float> OnFadeAudio;

        #endregion

        #region Methods

        // Unity Callbacks
        private void OnEnable()
        {
            OnFadeAudio += FadeAudio;
        }
         
        private void OnDisable()
        {
            OnFadeAudio -= FadeAudio;
        }
        
        private void Start()
        {
            FadeAudioEvent(isFadeIn: true);
        }
        
        // Core
        public static void FadeAudioEvent(bool isFadeIn, float duration = 0.5f)
        {
            OnFadeAudio?.Invoke(isFadeIn, duration);
        }
        
        public void FadeAudio(bool isFadeIn, float duration = 0.5f)
        {
            var audio = AudioManager.Instance.GetAudio(musicName);
            
            StartCoroutine(isFadeIn
                ? AudioHelpers.FadeIn(audio.source, duration, audio.volume)
                : AudioHelpers.FadeOut(audio.source, duration));
        }
        
        #endregion
    }
}