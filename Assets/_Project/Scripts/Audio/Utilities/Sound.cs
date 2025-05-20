using System;
using UnityEngine;

namespace ATBMI.Audio
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 255f)] 
        public int priority;
        [Range(0f, 1f)]
        public float volume;
        [Range(0f, 1f)]
        public float pitch;

        public bool loop;
        public bool sfx;

        [HideInInspector]
        public AudioSource source;
    }
}