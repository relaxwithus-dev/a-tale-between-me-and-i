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
        private SpriteRenderer _characterSr;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            InitOnAwake();
        }
        
        private void Start()
        {
            InitOnStart();
        }

        #endregion

        #region Methods

        // Initialize
        protected virtual void InitOnAwake()
        {
            _characterSr = GetComponentInChildren<SpriteRenderer>();
        }

        protected virtual void InitOnStart()
        {
            gameObject.name = Data.CharacterName;
        }
        
        // Core
        public void ChangeState(CharacterState state)
        {
            if (state == characterState) 
                return;
            
            characterState = state;
        }

        public void LookAt(Vector2 direction)
        {
            var directionX = characterDirection.x;
            characterDirection = direction;

            switch (directionX)
            {
                case > 0 when !isFacingRight:
                case < 0 when isFacingRight:
                    Flip();
                    break;
            }
        }
        
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            _characterSr.flipX = !_characterSr.flipX;
        }

        #endregion
    }
}
