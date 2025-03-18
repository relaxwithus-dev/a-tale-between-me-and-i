using UnityEngine;
using Sirenix.OdinInspector;
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

        [Space]
        [SerializeField] private InteractAction interactAction;
        [ShowIf("@this.interactAction == InteractAction.Give || this.interactAction == InteractAction.Take")]
        [SerializeField] private int targetId;
        [SerializeField] private Transform signTransform;
        [SerializeField] private Transform emojiTransform;

        private int _interactId;
        public bool IsInteracting => isInteracting;

        [Header("Reference")]
        [SerializeField] private CharacterTraits characterTraits;
        private CharacterAI _characterAI;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Awake()
        {
            _characterAI = GetComponent<CharacterAI>();
        }

        private void OnEnable()
        {
            InteractEvent.OnInteracted += cond => isInteracting = cond;
        }

        private void Start()
        {
            if (_characterAI.Data != null)
            {
                DialogueEvents.RegisterNPCTipTargetEvent(_characterAI.Data.CharacterName, GetSignTransform());
                DialogueEvents.RegisterNPCEmojiTargetEvent(_characterAI.Data.CharacterName, GetEmojiTransform());
            }
            else
            {
                Debug.LogError($"CharacterData is missing for NPC {gameObject.name}");
            }
        }

        public void Interact(InteractManager manager, int itemId = 0)
        {
            _interactId = itemId;
            if (_interactId == 0)
            {
                DialogueEvents.EnterDialogueEvent(_characterAI.Data.GetDefaultDialogue());
                characterTraits.InfluenceTraits(InteractAction.Talk);
            }
            else
            {
                // TODO: Saran lur, method baru bisa nge-pass 2 parameter, item id yg dipilih & target item id 
                var itemData = InventoryManager.Instance.GetItemData(_interactId);
                
                DialogueEvents.EnterItemDialogueEvent(_characterAI.Data.GetItemDialogue(itemData));
                characterTraits.InfluenceTraits(InteractAction.Give);
            }
        }

        // TODO: Pake ini buat change status di InkExternal
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

        // Helpers
        public Transform GetSignTransform() => signTransform;
        private Transform GetEmojiTransform() => emojiTransform;
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