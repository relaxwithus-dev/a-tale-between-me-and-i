using System;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Singleton;

namespace ATBMI.Audio
{
    public class AudioManager : MonoDDOL<AudioManager>
    {
        #region Fields & Properties
        
        [Header("Audio Data")]
        [SerializeField][Searchable] private Sound[] sounds;
        
        [Space]
        [SerializeField] private GameObject bgmContainer;
        [SerializeField] private GameObject sfxContainer;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Start()
        {
            InitializeAudio();
        }

        // Initialize
        private void InitializeAudio()
        {
            foreach (var s in sounds)
            {
                var container = s.sfx ? sfxContainer : bgmContainer;
                s.source = container.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        // Core 
        public void PlayAudio(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Bgm: {music} not found!");
                return;
            }

            if (sound.sfx)
                sound.source.PlayOneShot(sound.clip);
            else
                sound.source.Play();
        }

        public void StopAudio(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());

            sound.source.Stop();
        }

        public void PauseAudio(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            sound.source.Pause();
        }

        public void SetVolume(Musics music, float value)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            sound.source.volume = value;
        }

        public float GetVolume(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            return sound.volume;
        }

        public float GetDuration(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            return sound.clip.length;
        }

        // Utilities
        public Sound GetAudio(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            if (sound == null)
            {
                Debug.LogWarning($"Bgm: {music} not found!");
                return null;
            }
            return sound;
        }
        
        public bool IsAudioPlaying(Musics music)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == music.ToString());
            return sound.source.isPlaying;
        }
        
        // Set volume across all sounds
        // TODO: change to better method
        public void SetMasterVolume(float normalizedVolume)
        {
            AudioSource[] bgmSources = bgmContainer.GetComponents<AudioSource>();
            foreach (AudioSource audio in bgmSources)
            {
                audio.volume = normalizedVolume;
            }
        }

        #endregion
    }
}
