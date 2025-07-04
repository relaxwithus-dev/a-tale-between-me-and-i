using System.Collections;
using UnityEngine;
using ATBMI.Audio;

namespace ATBMI.Cutscene
{
    public class MusicCutscene : Cutscene
    {
        #region Fields
        
        [Header("Attribute")] 
        [SerializeField] private bool isPlayMusic;
        [SerializeField] private Musics cutsceneMusic = Musics.None;
        
        private float _fadeDuration;
        private Sound _currentMusic;
        
        #endregion

        #region Methods
        
        // Core
        public override void Execute()
        {
            if (cutsceneMusic == Musics.None)
            {
                Debug.LogWarning($"{cutsceneMusic} music is not set.");
                return;
            }
            
            StartCoroutine(isPlayMusic ? PlayCutsceneMusic() : StopCutsceneMusic());
        }
        
        private IEnumerator PlayCutsceneMusic()
        {
            var musics = GetMusic(cutsceneMusic);
            if (musics == null) yield break;
            
            _currentMusic = musics;
            AudioEvent.FadeOutAudioEvent();
            yield return new WaitForSeconds(_fadeDuration);
            
            yield return _currentMusic.source.FadeIn(0.5f, musics.volume);
            isFinishStep = true;
        }
        
        private IEnumerator StopCutsceneMusic()
        {
            var musics = GetMusic(cutsceneMusic);
            if (musics == null) yield break;
            
            isFinishStep = true;
            _currentMusic = musics;
            yield return _currentMusic.source.FadeOut(_fadeDuration);
            AudioEvent.FadeInAudioEvent();
        }
        
        private Sound GetMusic(Musics music)
        {
            var musics = AudioManager.Instance.GetAudio(music);
            if (musics == null)
            {
                Debug.LogWarning($"{cutsceneMusic} not found. Check Audio Manager instead!");
            }

            return musics;
        }
        
        #endregion
    }
}