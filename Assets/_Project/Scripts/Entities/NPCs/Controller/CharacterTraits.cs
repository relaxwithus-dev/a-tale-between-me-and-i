using UnityEngine;
using ATBMI.Enum;

namespace ATBMI.Entities.NPCs
{
    public class CharacterTraits : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Config")]
        [SerializeField] private InteractTypes currentAction;
        [SerializeField] private InfluenceConfiguration influenceConfig;
        [SerializeField] private PersonalityConfiguration personalityConfig;
        
        [Header("Traits")] 
        [SerializeField] private EmotionType dominantEmotion;
        [SerializeField] [Range(-1, 1)] private float[] currentEmotions;
        [SerializeField] [Range(-1, 1)] private float[] personalityTraits;
        
        private float[] _currentInfluence;
        
        #endregion

        #region Methods
        
        public void InfluenceTraits(InteractTypes types)
        {
            currentAction = types;
            _currentInfluence = influenceConfig.GetInfluenceValues(types);
            
            // Logic here
            // ..............
        }
        
        public void CalculateNewEmotion(float[] eventEmotions)
        {
            var newEmotions = new float[4];
            for (var i = 0; i < 4; i++)
            {
                var sum = 0f;
                for (var j = 0; j < 5; j++)
                {
                    var isPositive = eventEmotions[i] >= 0;
                    var factor = personalityConfig.GetPersonalityInfluence((PersonalityTrait)j,
                        (EmotionType)(i * 2), isPositive);
                    
                    sum += eventEmotions[i] * personalityTraits[j] * factor;
                }
                
                newEmotions[i] = sum / 5f;
            }
            
            // Update current emotions
            for (var i = 0; i < 4; i++)
            {
                currentEmotions[i] += Mathf.Clamp(newEmotions[i], -1f, 1f);
            }
            
            ExtractDominantEmotion();
        }
        
        // Extract the most influential emotion following the algorithm from PDF
        private void ExtractDominantEmotion()
        {
            // Find largest absolute value
            var maxValue = 0f;
            foreach (var emo in currentEmotions)
            {
                maxValue = Mathf.Max(maxValue, Mathf.Abs(emo));
            }
            
            // Compare with largest value and zero out others
            for (var i = 0; i < currentEmotions.Length; i++)
            {
                if (Mathf.Abs(currentEmotions[i]) != maxValue)
                {
                    currentEmotions[i] = 0f;
                }
            }

            // Round values according to rules
            var nonZeroCount = 0;
            foreach (var emotion in currentEmotions)
            {
                if (emotion != 0)
                    nonZeroCount++;
            }
            
            if (nonZeroCount == 2)
            {
                // If two values are greater than zero, both become 0.5
                for (var i = 0; i < currentEmotions.Length; i++)
                {
                    if (currentEmotions[i] != 0)
                        currentEmotions[i] = 0.5f * Mathf.Sign(currentEmotions[i]);
                }
            }
            else
            {
                // Apply rounding rules
                for (var i = 0; i < currentEmotions.Length; i++)
                {
                    var absValue = Mathf.Abs(currentEmotions[i]);
                    var sign = Mathf.Sign(currentEmotions[i]);

                    currentEmotions[i] = absValue switch
                    {
                        <= 0.1f => 0f,
                        <= 0.3f => 0.2f * sign,
                        <= 0.5f => 0.5f * sign,
                        _ => 1.0f * sign
                    };
                }
            }
        }
        
        // Helper method to get the current dominant emotion
        public (EmotionType, float) GetDominantEmotion()
        {
            float maxValue = 0f;
            int maxIndex = 0;
            
            for (var i = 0; i < currentEmotions.Length; i++)
            {
                if (Mathf.Abs(currentEmotions[i]) > Mathf.Abs(maxValue))
                {
                    maxValue = currentEmotions[i];
                    maxIndex = i;
                }
            }
            
            return (maxValue >= 0 ? (EmotionType)(maxIndex * 2) : (EmotionType)(maxIndex * 2 + 1), maxValue);
        }
        
        #endregion
        
    }
}