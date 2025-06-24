using ATBMI.Data;
using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI.Dialogue
{
    public class ObjectPinRegisterer : MonoBehaviour
    {
        [SerializeField] private CharacterData npcDataReference;
        [SerializeField] private Transform emojiPin;

        private void Start()
        {
            DialogueEvents.RegisterNPCTipTargetEvent(npcDataReference.CharacterName, transform);
            DialogueEvents.RegisterNPCEmojiTargetEvent(npcDataReference.CharacterName, transform);
        }
    }
}
