using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;
using ATBMI.Scene;
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
        [SerializeField] [MaxValue(40f)] private float stressValue;

        [Space]
        [SerializeField] private InteractAction interactAction;
        [SerializeField] private Transform signTransform;
        [SerializeField] private Transform emojiTransform;

        private int _interactCount;
        private TextAsset _currentDialogue;
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
        
        private void Start()
        {
            // Stats
            _interactCount = 0;
            isInteracting = false;
            interactAction = InteractAction.Talk;
            
            // Register
            DialogueEvents.RegisterNPCTipTargetEvent(_characterAI.Data.CharacterName, GetSignTransform());
            DialogueEvents.RegisterNPCEmojiTargetEvent(_characterAI.Data.CharacterName, GetEmojiTransform());
            QuestEvents.RegisterThisNPCToHandledByQuestStepEvent(_characterAI);
        }
        
        // TODO: Meet needed, gimana cara cek kalau interacting sudah selesai?
        public void Interact(int itemId = 0)
        {
            isInteracting = true;
            InteractObserver.Observe(this);
            if (itemId == 0)
            {
                var sceneId = SceneNavigation.Instance.CurrentScene.Id;
                var dialogues = _characterAI.Data.GetDefaultDialogues(sceneId);
                
                _interactCount = Mathf.Clamp(_interactCount++, 0, dialogues.Length - 1);
                characterTraits.InfluenceTraits(InteractAction.Talk);
                
                DialogueEvents.EnterDialogueEvent(dialogues[_interactCount]);
                StressEvents.StressOnceEvent(isAddStress: true, stressValue);
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