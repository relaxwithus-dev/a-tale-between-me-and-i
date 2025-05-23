using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckFatigue : Leaf
    {
        private readonly float stamina;
        
        private float _currentStamina;
        private bool _isFatigue;

        public CheckFatigue(float stamina)
        {
            this.stamina = stamina;
            _currentStamina = stamina;
        }
        
        public override NodeStatus Evaluate()
        {
            if (!_isFatigue)
            {
                Debug.Log("Execute Success: CheckFatigue");
                return NodeStatus.Success;
            }
            
            Debug.LogWarning("Execute Failure: CheckFatigue");
            return NodeStatus.Failure;
        }

        protected override void Reset()
        {
            base.Reset();
            _isFatigue = false;
            _currentStamina = stamina;
        }

        public void ModifyStamina()
        {
            if (_isFatigue) return;
            
            _currentStamina -= Time.deltaTime;
            if (_currentStamina <= 0)
            {
                _isFatigue = true;
                _currentStamina = stamina;
            }
        }
        
    }
}