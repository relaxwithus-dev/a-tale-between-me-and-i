using System;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Data;
using ATBMI.Enum;
using ATBMI.Inventory;
using ATBMI.Entities.NPCs;
using ATBMI.Gameplay.Event;

namespace ATBMI.Interaction
{
    public class CharacterInteract : MonoBehaviour, IInteractable
    {
        #region Fields & Properties

        [Header("Properties")]
        [SerializeField] private bool isInteracting;
        [SerializeField] private InteractAction interactAction;
        [ShowIf("@this.interactAction == InteractAction.Give || this.interactAction == InteractAction.Take")]
        [SerializeField] private int targetId;
        [SerializeField] private Transform signTransform;
        [SerializeField] private Transform emojiTransform;

        private int _interactId;
        public bool IsInteracting => isInteracting;
        public static event Action<bool> OnInteracting;

        [Header("Reference")]
        [SerializeField] private CharacterTraits characterTraits;
        [SerializeField] private CharacterAI characterAI;

        #endregion

        #region Methods

        private void OnEnable()
        {
            OnInteracting += cond => isInteracting = cond;
        }

        private void Start()
        {
            if (characterAI.Data != null)
            {
                DialogueEvents.RegisterNPCTipTargetEvent(characterAI.Data.CharacterName, GetSignTransform());
                DialogueEvents.RegisterNPCEmojiTargetEvent(characterAI.Data.CharacterName, GetEmojiTransform());
            }
            else
            {
                Debug.LogError($"CharacterData is missing for NPC {gameObject.name}");
            }
        }

        public static void InteractingEvent(bool isBegin) => OnInteracting?.Invoke(isBegin);
        public Transform GetSignTransform() => signTransform;
        public Transform GetEmojiTransform() => emojiTransform;

        // TODO: Adjust isi method dibawah sesuai dgn jenis interaksi
        public void Interact(InteractManager manager, int itemId = 0)
        {
            _interactId = itemId;
            if (_interactId == 0)
            {
                DialogueEvents.EnterDialogueEvent(characterAI.Data.GetDefaultDialogue());
                characterTraits.InfluenceTraits(InteractAction.Run);
            }
            else
            {
                // TODO: Saran lur, method baru bisa nge-pass 2 parameter, item id yg dipilih & target item id 
                ItemData itemData = InventoryManager.Instance.GetItemData(_interactId);
                TextAsset itemDialogue = characterAI.Data.GetItemDialogue(itemData);
                DialogueEvents.EnterItemDialogueEvent(itemDialogue);
            }
        }

        // TODO: Pake ini buat change status di InkExternal
        // NOTE: Pake method waktu diawal interaksi, sesuaiken dgn jenis interaksinya,
        // Semua Ink Dialogue NPCs yang punya emosi, harus pake method ini
        public void ChangeStatus(string action)
        {
            var changedAction = GetAction(action);
            characterTraits.InfluenceTraits(changedAction);
            
            if (interactAction == changedAction) return;
            interactAction = changedAction;
        }

        // TODO: Pake ini buat check match id di (Kalo jenis interaksi give/take)
        public bool IsMatchId()
        {
            var isMatch = _interactId == targetId;
            if (isMatch)
            {
                _interactId = 0;
                return true;
            }
            return false;
        }

        private InteractAction GetAction(string action)
        {
            if (System.Enum.TryParse<InteractAction>(action, out var parsedAction))
            {
                var allActions = System.Enum.GetValues(typeof(InteractAction));
                foreach (InteractAction enumAction in allActions)
                {
                    if (enumAction == parsedAction)
                    {
                        return enumAction;
                    }
                }
            }
            return InteractAction.Talk;
        }

        #endregion
    }
}