using System;

namespace ATBMI.Audio
{
    public static class AudioEvent
    {
        public static event Action OnFadeInAudio;
        public static event Action OnFadeOutAudio;
        
        public static void FadeInAudioEvent() => OnFadeInAudio?.Invoke();
        public static void FadeOutAudioEvent() => OnFadeOutAudio?.Invoke();
    }
}