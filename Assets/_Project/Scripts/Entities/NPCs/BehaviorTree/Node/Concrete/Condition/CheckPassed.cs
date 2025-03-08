using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckPassed : Leaf
    {
        private readonly CharacterAI character;
        private readonly float offRange;
        
        private const float OFFSET = 2f;
        
        private Transform _currentTarget;
        private Vector3 _passedPosition;
        
        public CheckPassed(CharacterAI character, float offRange)
        {
            this.character = character;
            this.offRange = offRange;
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            if (!CheckOnRange())
                return NodeStatus.Failure;
            
            return Vector3.Distance(character.transform.position, _passedPosition) <= 0.01f
                ? NodeStatus.Success
                : NodeStatus.Running;
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
            var offset = character.transform.position.x > target.transform.position.x ? -OFFSET : OFFSET;
            _passedPosition = new Vector3(character.transform.position.x + offset,
                character.transform.position.y,
                character.transform.position.z);
            
            Debug.Log(offset);
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