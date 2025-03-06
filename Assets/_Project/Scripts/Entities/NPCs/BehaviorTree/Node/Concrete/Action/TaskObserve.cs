using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskObserve : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterAnimation animation;
        private readonly float outRange;
        
        private Transform _currentTarget;
        private Vector3 _targetPosition;

        public TaskObserve(CharacterAI character, CharacterAnimation animation, float outRange)
        {
            this.character = character;
            this.animation = animation;
            this.outRange = outRange;
            
            InitFactors(plan: 1, risk: 0.3f, timeRange: (3f, 9f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            if (Vector3.Distance(character.transform.position, _currentTarget.transform.position) >= outRange)
                return NodeStatus.Success;
            
            _targetPosition = _currentTarget.transform.position - character.transform.position;
            _targetPosition.Normalize();
            character.LookAt(_targetPosition);
            animation.TrySetAnimationState("Observe");
            
            return NodeStatus.Running;
        }
        
        private bool TrySetupTarget()
        {
            if (_currentTarget != null)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (target == null)
            {
                Debug.LogWarning("Execute Failure: TaskObserve");
                return false;
            }
            
            Debug.Log("Execute Success: TaskObserve");
            _currentTarget = target;
            return true;
        }
    }
}