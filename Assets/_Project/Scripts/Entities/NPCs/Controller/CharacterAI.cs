using System;
using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class CharacterAI : MonoBehaviour, IController
    {
        #region Fields & Properties

        [Header("General")]
        [SerializeField] protected CharacterData characterData;
        [SerializeField] protected EntitiesState characterState;
        [SerializeField] private Vector2 characterDirection;
        [SerializeField] private bool isFacingRight;
        
        public CharacterData Data => characterData;
        public EntitiesState State => characterState;
        public Vector2 Direction => characterDirection;
        public bool IsFacingRight => isFacingRight;
        
        // Reference
        private CharacterAnimation _characterAnim;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _characterAnim = GetComponentInChildren<CharacterAnimation>();
        }

        private void Start()
        {
            // Init stats
            var direction = IsFacingRight ? 1 : -1;
            var angle = isFacingRight ? 0f : 180f;
            
            transform.Rotate(0f, angle, 0f);
            characterState = EntitiesState.Idle;
            characterDirection = new Vector2(direction, 0);
        }

        // Core
        public void ChangeState(EntitiesState state)
        {
            characterState = state;
            _characterAnim.TrySetAnimationState(state.ToString());
        }
        
        public void LookAt(Vector2 direction)
        {
            characterDirection = direction;
            characterDirection.Normalize();
            if (direction.x > 0 && !isFacingRight || direction.x < 0 && isFacingRight)
                Flip();
        }
        
        public void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        
        #endregion
    }
}
