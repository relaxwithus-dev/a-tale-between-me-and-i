using UnityEngine;
using ATBMI.Enum;
using ATBMI.Inventory;
using ATBMI.Entities.NPCs;
using ATBMI.Gameplay.Event;

namespace ATBMI.Interaction
{
    public class CharacterInteract : MonoBehaviour, IInteractable
    {
        #region Fields & Properties
        
        [Header("Stats")]
        [SerializeField] private bool isInteracting;
        
        [Space]
        [SerializeField] private InteractAction interactAction;
        [SerializeField] private Transform signTransform;
        [SerializeField] private Transform emojiTransform;
        
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
            if (_characterAI.Data == null)
            {
                Debug.LogError($"npc {gameObject.name} data is missing");
                return;
            }
            
            // Register
            DialogueEvents.RegisterNPCTipTargetEvent(_characterAI.Data.CharacterName, GetSignTransform());
            DialogueEvents.RegisterNPCEmojiTargetEvent(_characterAI.Data.CharacterName, GetEmojiTransform());
        }
        
        // TODO: Meet needed, gimana cara cek kalau interacting sudah selesai?
        public void Interact(InteractManager manager, int itemId = 0)
        {
            isInteracting = true;
            InteractObserver.Observe(this);
            if (itemId == 0)
            {
                DialogueEvents.EnterDialogueEvent(_characterAI.Data.GetDefaultDialogueByScene());
                characterTraits.InfluenceTraits(InteractAction.Talk);
            }
            else
            {
                var itemData = InventoryManager.Instance.GetItemData(itemId);
                DialogueEvents.EnterItemDialogueEvent(_characterAI.Data.GetItemDialogue(itemData));
            }
        }
        
        public void ChangeStatus(string action)
        {
            var changedAction = GetAction(action);
            characterTraits.InfluenceTraits(changedAction);

            if (interactAction == changedAction) return;
            interactAction = changedAction;
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