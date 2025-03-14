using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [CreateAssetMenu(fileName = "NewPersonalityConfig", menuName = "Data/Entities/Config/Personality Configuration", order = 0)]
    public class PersonalityConfiguration : ScriptableObject
    {
        private float[,] positiveEmotionMatrix = new float[5, 4]
        {
            { -1f, 1f, 1f, -1f }, // Openness influences
            { 0f, 0f, 0f, 0f }, // Conscientiousness influences
            { 0f, 1f, 1f, 1f }, // Extraversion influences
            { 0f, 1f, 1f, 1f }, // Agreeableness influences
            { -1f, -1f, -1f, 1f } // Neuroticism influences
        };

        private float[,] negativeEmotionMatrix = new float[5, 4]
        {
            { 0f, -1f, -1f, -1f }, // Openness influences
            { -1f, 0f, 1f, 0f }, // Conscientiousness influences
            { 0f, 0f, 1f, -1f }, // Extraversion influences
            { 0f, -1f, 0f, -1f }, // Agreeableness influences
            { 1f, 1f, 1f, 1f } // Neuroticism influences
        };

        public float GetPersonalityInfluence(PersonalityTrait trait, Emotion emotion, bool isPositive)
        {
            var traitIndex = (int)trait;
            var emotionIndex = (int)emotion / 2;
            return isPositive
                ? positiveEmotionMatrix[traitIndex, emotionIndex]
                : negativeEmotionMatrix[traitIndex, emotionIndex];
        }
    }
}