using System;
using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class CharacterAI : MonoBehaviour
    {
        #region Fields & Properties

        [Header("General")]
        [SerializeField] protected CharacterData characterData;
        [SerializeField] protected CharacterState characterState;
        [SerializeField] private Vector2 characterDirection;
        [SerializeField] private bool isFacingRight;
        
        public CharacterData Data => characterData;
        public CharacterState State => characterState;
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
            gameObject.name = Data.CharacterName;
        }
        
        // Core
        public void ChangeState(CharacterState state)
        {
            if (state == characterState) 
                return;
            
            characterState = state;
            _characterAnim.SetupAnimationState(state.ToString());
        }
        
        public void LookAt(Vector2 direction)
        {
            characterDirection = direction.normalized;
            if (direction.x > 0 && !isFacingRight || direction.x < 0 && isFacingRight)
                Flip();
        }
        
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }

        #endregion
    }
}
