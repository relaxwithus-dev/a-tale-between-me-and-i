using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;
using ATBMI.Gameplay.Event;

namespace ATBMI.Interaction
{
    public class CharacterInteraction : Interaction
    {
        #region Fields & Properties
        
        [SerializeField] private InteractSelect interactSelect;
        [ShowIf("@this.interactSelect != InteractSelect.None")]
        [SerializeField] private int targetId;
        
        private int _interactId;
        public int TargetId => targetId;
        
        #endregion
        
        #region Methods
        
        // TODO: Adjust isi method dibawah sesuai dgn jenis interaksi
        public override void Interact(InteractManager manager, int itemId = 0)
        {
            base.Interact(manager, itemId);
            _interactId = itemId;
            
            if (_interactId == 0)
            {
                DialogEvents.EnterDialogueEvent();
            }
            else
            {
                // TODO: Saran lur, method baru bisa nge-pass 2 parameter, item id yg dipilih & target item id 
                // DialogEvents.EnterDialogueEvent(_interactId, targetItemId);
            }
        }
        
        // TODO: Pake ini buat change status di InkExternal
        public void ChangeStatus(InteractSelect status)
        {
            if (interactSelect == status) return;
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