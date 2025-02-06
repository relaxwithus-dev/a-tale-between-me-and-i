using System;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;
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
        
        private int _interactId;
        public static event Action<bool> OnInteracting; 
        
        #endregion
        
        #region Methods
        
        private void OnEnable()
        {
            OnInteracting += cond => isInteracting = cond;
        }
        
        public static void InteractingEvent(bool isBegin) => OnInteracting?.Invoke(isBegin);

        // TODO: Adjust isi method dibawah sesuai dgn jenis interaksi
        public void Interact(InteractManager manager, int itemId = 0)
        {
            _interactId = itemId;
            if (_interactId == 0)
            {
                DialogueEvents.EnterDialogueEvent();
            }
            else
            {
                // TODO: Saran lur, method baru bisa nge-pass 2 parameter, item id yg dipilih & target item id 
                // DialogueEvents.EnterDialogueEvent(_interactId, targetItemId);
            }
        }
        
        // TODO: Pake ini buat change status di InkExternal
        public void ChangeStatus(InteractAction status)
        {
            if (interactAction == status)
                return;
            
            interactAction = status;
        }
        
        // TODO: Pake ini buat check match id di InkExternal
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
        
        #endregion
    }
}