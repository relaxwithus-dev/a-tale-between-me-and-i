using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;

namespace ATBMI.Entities.NPCs
{
    public class CharacterTraits : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Config")]
        [SerializeField] private InfluenceConfiguration influenceConfig;
        [SerializeField] private PersonalityConfiguration personalityConfig;

        [Header("Traits")]
        [SerializeField] private Emotion initialEmotion;
        [SerializeField] [Range(-1, 1)] [ReadOnly] private float[] emotions = new float[4];
        [SerializeField] [Range(-1, 1)] [ReadOnly] private float[] personality = new float[5];

        private float[] _eventEmotion;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            _eventEmotion = new float[4];
            
            var emotionIndex = (int)initialEmotion;
            var isPositiveEmotion = emotionIndex % 2 == 0;
            
            emotions[emotionIndex / 2] = isPositiveEmotion ? 0.5f : -0.5f;
            personality = personalityConfig.GetPersonalityInfluence(initialEmotion, isPositiveEmotion);
        }
        
        // Core
        public void InfluenceTraits(InteractAction action)
        {
            var influence = influenceConfig.GetInfluenceValues(action);
            if (!ValidateEmotion(influence)) 
                return;
            
            for (var i = 0; i < emotions.Length; i++)
            {
                _eventEmotion[i] = emotions[i] + influence[i];
            }
            CalculateNewEmotion();
        }
        
        private void CalculateNewEmotion()
        {
            var newEmotions = new float[4];
            for (var i = 0; i < 4; i++)
            {
                var sum = 0f;
                for (var j = 0; j < 5; j++)
                {
                    var isPositive = _eventEmotion[i] >= 0;
                    float factor = personalityConfig.GetPersonalityTrait((PersonalityTrait)j,
                        (Emotion)(i * 2), isPositive);
                    
                    sum += _eventEmotion[i] * personality[j] * factor;
                }
                
                newEmotions[i] = sum / 5f;
            }
            
            // Update current emotions
            for (var i = 0; i < 4; i++)
            {
                emotions[i] = Mathf.Clamp(emotions[i] + newEmotions[i], -1f, 1f);
            }
            
            ExtractDominantEmotion();
        }
        
        private void ExtractDominantEmotion()
        {
            // Find largest absolute value
            var maxValue = 0f;
            for (var i = 0; i < emotions.Length; i++)
            {
                maxValue = Mathf.Max(maxValue, Mathf.Abs(emotions[i]));
            }
            
            // Compare with largest value and zero out others
            for (var i = 0; i < emotions.Length; i++)
            {
                if (!Mathf.Approximately(Mathf.Abs(emotions[i]), maxValue))
                {
                    emotions[i] = 0f;
                }
            }
            
            // Round values according to rules
            var nonZeroCount = 0;
            for (var i = 0; i < emotions.Length; i++)
            {
                if (emotions[i] != 0)
                    nonZeroCount++;
            }

            if (nonZeroCount == 2)
            {
                // If two values are greater than zero, both become 0.5
                for (var i = 0; i < emotions.Length; i++)
                {
                    if (emotions[i] != 0)
                        emotions[i] = 0.5f * Mathf.Sign(emotions[i]);
                }
            }
            else
            {
                // Apply rounding rules
                for (var i = 0; i < emotions.Length; i++)
                {
                    float absValue = Mathf.Abs(emotions[i]);
                    float sign = Mathf.Sign(emotions[i]);

                    emotions[i] = absValue switch
                    {
                        <= 0.1f => 0f,
                        <= 0.3f => 0.2f * sign,
                        <= 0.5f => 0.5f * sign,
                        _ => 1.0f * sign
                    };
                }
            }
        }
        
        // Helpers
        public (Emotion, float) GetDominantEmotion()
        {
            var maxValue = 0f;
            var maxIndex = 0;
            
            for (var i = 0; i < emotions.Length; i++)
            {
                if (Mathf.Abs(emotions[i]) > Mathf.Abs(maxValue))
                {
                    maxValue = emotions[i];
                    maxIndex = i;
                }
            }
            
            return (maxValue >= 0 ? (Emotion)(maxIndex * 2) : (Emotion)(maxIndex * 2 + 1), maxValue);
        }
        
        private bool ValidateEmotion(float[] influence)
        {
            var isValid = emotions.Length == influence.Length;
            if (!isValid)
            {
                Debug.LogError("influence count mismatch!");
                return false;
            }
            
            return true;
        }
        
        #endregion
        
    }
}