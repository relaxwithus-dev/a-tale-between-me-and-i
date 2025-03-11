using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskObserve : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterAnimation animation;
        private readonly float offRange;

        private const float OFFSET = 0.5f;
        private Transform _currentTarget;
        private Vector3 _targetPosition;

        public TaskObserve(CharacterAI character, CharacterAnimation animation, float offRange)
        {
            this.character = character;
            this.animation = animation;
            this.offRange = offRange;
            
            InitFactors(plan: 1f, risk: 0.2f, timeRange: (3f, 7f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;

            if (CheckOffRange())
            {
                Debug.Log("Execute Success: TaskObserve");
                return NodeStatus.Success;
            }
            
            _targetPosition = character.transform.position - _currentTarget.transform.position;
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
            
            _currentTarget = target;
            return true;
        }
        
        private bool CheckOffRange()
        {
            var characterX = character.transform.position.x;
            var targetX = _currentTarget.transform.position.x;
            var bound = offRange + OFFSET;
            
            return targetX < characterX - bound || targetX > characterX + bound;
        }
    }
}