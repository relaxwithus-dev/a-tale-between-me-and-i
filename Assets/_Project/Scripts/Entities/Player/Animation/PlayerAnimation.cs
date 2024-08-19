using System;
using UnityEngine;
using ATBMI.Enum;

namespace ATBMI.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region Fields & Properties
        
        // Cached properties
        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Run = Animator.StringToHash("Run");

        // Reference
        private PlayerController _playerController;
        private Animator _playerAnim;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerAnim = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            AnimationStateHandler();
        }

        #endregion

        #region Methods

        // !- Core
        private void AnimationStateHandler()
        {
            var state = GetState();

            if (state == _currentState) return;
            _playerAnim.CrossFade(state, 0, 0);
            _currentState = state;
        }

        private int GetState()
        {
            var playerState = _playerController.PlayerState;
            return playerState switch
            {
                PlayerState.Run => Run,
                PlayerState.Walk => Walk,
                PlayerState.Idle => Idle,
                _ => throw new InvalidOperationException("Invalid player state"),
            };
        }

        #endregion
    }
}