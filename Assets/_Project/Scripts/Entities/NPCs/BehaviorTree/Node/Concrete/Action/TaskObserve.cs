using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskObserve : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterAnimation animation;
        private readonly float offRange;
        
        private Transform _currentTarget;
        private Vector3 _targetPosition;

        public TaskObserve(CharacterAI character, CharacterAnimation animation, float offRange)
        {
            this.character = character;
            this.animation = animation;
            this.offRange = offRange;
            
            InitFactors(plan: 1, risk: 0.3f, timeRange: (3f, 9f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;

            if (!CheckOnRange())
                return NodeStatus.Failure;
            
            _targetPosition = _currentTarget.transform.position - character.transform.position;
            _targetPosition.Normalize();
            character.LookAt(_targetPosition);
            animation.TrySetAnimationState("Observe");
            
            return NodeStatus.Running;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentTarget = null;
            _targetPosition = Vector3.zero;
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

        private bool CheckOnRange()
        {
            var characterX = character.transform.position.x;
            var targetX = _currentTarget.transform.position.x;

            return targetX < characterX - offRange || targetX > characterX + offRange;
        }
    }
}