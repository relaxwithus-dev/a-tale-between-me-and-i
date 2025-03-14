using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckPassed : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float offRange;
        
        private const float OFFSET = 0.5f;
        private const float PASS_OFFSET = 1.3f;
        
        private Transform _currentTarget;
        private Vector3 _passedPosition;
        
        public CheckPassed(CharacterAI character, float offRange)
        {
            this.character = character;
            this.offRange = offRange;
            
            InitFactors(plan: 0, risk: 0.1f, timeRange:(0.5f, 1.3f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;

            if (CheckOffRange())
                return NodeStatus.Failure;
            
            if (!(Vector3.Distance(_currentTarget.transform.position, _passedPosition) <= 0.1f))
                return NodeStatus.Running;
            
            Debug.LogWarning("Execute Success: CheckPassed");
            return NodeStatus.Success;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentTarget = null;
            _passedPosition = Vector3.zero;
        }

        private bool TrySetupTarget()
        {
            if (_currentTarget != null)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (target == null)
            {
                Debug.LogWarning("Execute Failure: CheckPassed");
                return false;
            }
            
            _currentTarget = target;
            var offset = target.transform.position.x > character.transform.position.x ? -PASS_OFFSET : PASS_OFFSET;
            _passedPosition = new Vector3(character.transform.position.x + offset,
                character.transform.position.y,
                character.transform.position.z);
            
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