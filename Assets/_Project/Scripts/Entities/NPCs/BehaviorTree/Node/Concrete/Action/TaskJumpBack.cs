using UnityEngine;
using DG.Tweening;

namespace ATBMI.Entities.NPCs
{
    public class TaskJumpBack : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float power;
        private readonly float duration;
        private readonly float distance = 0.15f;
        
        private Vector3 _jumpTarget;
        
        public TaskJumpBack(CharacterAI character, float power, float duration)
        {
            this.character = character;
            this.power = power;
            this.duration = duration;
            
            InitFactors(plan: 1, risk: 0.6f, timeRange: (0.4f, 0.8f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            character.transform.DOJump(_jumpTarget, power, 1, duration).SetEase(Ease.OutSine);
            character.LookAt(_jumpTarget);
            
            Debug.Log("Execute Success: TaskJumpBack");
            parentNode.ClearData(TARGET_KEY);
            return NodeStatus.Success;
        }

        protected override void Reset()
        {
            base.Reset();
            _jumpTarget = Vector3.zero;
        }

        private bool TrySetupTarget()
        {
            if (_jumpTarget != Vector3.zero)
                return true;

            var target = (Transform)GetData(TARGET_KEY);
            if (!target)
            {
                Debug.LogWarning("Execute Failure: TaskJumpBack");
                return false;
            }
            
            var currentDirection = character.transform.position.x > target.position.x ? 1f : -1f;
            var pointThreshold = currentDirection * distance;
            _jumpTarget = new Vector3(character.transform.position.x + pointThreshold,
                character.transform.position.y,
                character.transform.position.z);
            
            return true;
        }
    }
}