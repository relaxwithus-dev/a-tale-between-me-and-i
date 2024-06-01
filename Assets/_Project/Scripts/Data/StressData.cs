using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewStressData", menuName = "Data/Stress Data", order = 0)]
    public class StressData : ScriptableObject
    {
        [Header("General")]
        [SerializeField] [Range(0, 100)] private float speedPercentage;
        [SerializeField] private AudioClip stressAudio;
        [SerializeField] private TextAsset dialogueAsset;

        public float SpeedPercentage => speedPercentage;
        public AudioClip StressAudio => stressAudio;
        public TextAsset DialogueAsset => dialogueAsset;
    }
}
