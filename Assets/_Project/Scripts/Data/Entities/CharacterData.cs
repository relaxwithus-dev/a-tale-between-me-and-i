using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Entities.NPCs;
using ATBMI.Scene;
using UnityEngine.Serialization;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Entities/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        #region Struct

        [Serializable]
        private struct MoveStats
        {
            public string Type;
            public float Speed;
        }
        
        [Serializable]
        private class Dialogues
        {
            public Location location;
            public TextAsset[] dialogues;
        }
        
        [Serializable]
        private class EmotionDialogues
        {
            public Emotion emotion;
            public TextAsset[] dialogues;
        }
        
        #endregion

        private enum CharacterType { Normal, Emotion, Story }
        
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private string animationTag;
        [SerializeField] [EnumToggleButtons] private CharacterType type;
        [SerializeField] private bool isIdling;
        [SerializeField] [HideIf("isIdling")] 
        private MoveStats[] moveSpeeds;
        
        [Header("Dialogue")]
        [SerializeField] private List<Dialogues> defaultDialogues;
        [SerializeField] [ShowIf("type", CharacterType.Emotion)]
        private List<EmotionDialogues> emotionDialogues;
        [SerializeField] private List<ItemDialogue> itemDialogues;
        
        // Stats
        public string CharacterName => characterName;
        public string AnimationTag => animationTag;
        public float GetSpeedByType(string type)
        {
            foreach (var speed in moveSpeeds)
            {
                if (speed.Type == type)
                    return speed.Speed;
            }
            
            Debug.LogError("move speeds not found!");
            return 0f;
        }
        
        // Dialogue
        public TextAsset[] GetDefaultDialogues(Location location = Location.Default)
        {
            if (defaultDialogues == null || defaultDialogues.Count == 0)
            {
                Debug.LogError("Default dialogues not set!");
                return null;
            }

            // If location is default, fallback immediately
            if (location == Location.Default)
                return defaultDialogues[0].dialogues;

            // Try find the dialogue
            var found = defaultDialogues.Find(d => d.location == location);
            return found != null ? found.dialogues : defaultDialogues[0].dialogues;
        }
        
        public TextAsset[] GetEmotionDialogues(Emotion emotion)
        {
            if (emotionDialogues == null || emotionDialogues.Count == 0)
            {
                Debug.LogError("Default dialogues not set!");
                return null;
            }
            
            // Try find the dialogue
            var asset = emotionDialogues.Find(x => x.emotion == emotion);
            return asset != null ? asset.dialogues : emotionDialogues[0].dialogues;
        }
        
        public TextAsset GetItemDialogue(ItemData item)
        {
            // Try find the dialogue
            var entry = itemDialogues.Find(d => d.item == item);
            return entry != null ? entry.dialogue : itemDialogues[0].dialogue;
        }
    }
}
