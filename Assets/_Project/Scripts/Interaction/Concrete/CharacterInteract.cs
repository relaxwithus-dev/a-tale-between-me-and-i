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
        [SerializeField] private InteractSelect interactSelect;
        [ShowIf("@this.interactSelect != InteractSelect.None")]
        [SerializeField] private int targetId;
        [SerializeField] private Transform signTransform;
        
        private int _interactId;
        public static event Action<bool> OnInteracting; 
        
        #endregion
        
        #region Methods
        
        private void OnEnable()
        {
            OnInteracting += cond => isInteracting = cond;
        }
        
        public static void InteractingEvent(bool isBegin) => OnInteracting?.Invoke(isBegin);

        public Transform GetSignTransform() => signTransform;

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
        public void ChangeStatus(InteractSelect status)
        {
            if (interactSelect == status)
                return;
            
            interactSelect = status;
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