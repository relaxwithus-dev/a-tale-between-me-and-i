using System;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour
    {
        #region Fields & Properties
        
        private int _currentState;
        
        // Cached properties
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Dialogue = Animator.StringToHash("Dialogue");
        
        // Reference
        private CharacterAI _characterAI;
        private Animator _characterAnim;

        #endregion
         
        #region Unity Methods
            
        private void Awake()
        {
            _characterAI = GetComponentInParent<CharacterAI>();
            _characterAnim = GetComponent<Animator>();
        }

        private void Update()
        {
            AnimationStateHandler();
        }

        #endregion

        #region  Methods
        
        private void AnimationStateHandler()
        {
            var state = GetState();

            if (state == _currentState) return;
            _characterAnim.CrossFade(state, 0, 0);
            _currentState = state;
        }
        
        private int GetState()
        {
            return _characterAI.State switch
            {
                CharacterState.Idle => Idle,
                CharacterState.Walk => Walk,
                CharacterState.Run => Run,
                CharacterState.Talk => Dialogue,
                _ => throw new InvalidOperationException("Invalid NPCs State!")
            };
        }

        #endregion
    }
}