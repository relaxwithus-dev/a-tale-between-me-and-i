using System.Collections;
using UnityEngine;

namespace ATBMI.Audio
{
    public static class AudioHelpers
    {
        public static IEnumerator CrossFade(this AudioSource fromAudio, AudioSource toAudio, 
            float targetVolume, float fadeTime)
        {
            yield return FadeOut(fromAudio, fadeTime);
            yield return FadeIn(toAudio, fadeTime, targetVolume);
        }
        
        public static IEnumerator FadeOut(this AudioSource audio, float fadeTime)
        {
            var startVolume = audio.volume;
            while (audio.volume > 0)
            {
                audio.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
            audio.Stop();
            audio.volume = 0;
        }

        public static IEnumerator FadeIn(this AudioSource audio, float fadeTime, float targetVolume)
        {
            const float startVolume = 0.2f;
            audio.volume = 0;
            audio.Play();
            while (audio.volume < targetVolume)
            {
                audio.volume += startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
            audio.volume = targetVolume;
        }
    }
}