using System;
using System.Collections;
using System.Collections.Generic;
using ATBMI.Data;
using KevinCastejon.MoreAttributes;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Struct

        [Serializable]
        public struct SpeedStats
        {
            public float MoveSpeed;
            public float Acceleration;
            public float Decceleration;
        }

        #endregion

        #region Const Variable

        private const string IDLE_STATE = "Idle";
        private const string WALK_STATE = "Walk";
        private const string RUN_STATE = "Run";

        private const string IS_MOVE = "isMove";

        #endregion

        #region Fields & Property

        [Header("Data")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private string playerName;
        [SerializeField] private bool isRight;

        private bool _canMove;

        public PlayerData PlayerData => playerData;

        [Header("State")]
        private PlayerStateSwitcher _playerStateController;
        public IdleState IdleState { get; private set; }
        public WalkState WalkState { get; private set; }
        public RunState RunState { get; private set; }

        [Header("Reference")]
        private PlayerInputHandler _playerInputHandler;
        private Animator _playerAnimator;
        public PlayerInputHandler PlayerInputHandler => _playerInputHandler;
        public Animator PlayerAnimator => _playerAnimator;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // Component
            _playerAnimator = GetComponentInChildren<Animator>();
            _playerInputHandler = GetComponentInChildren<PlayerInputHandler>();

            // State
            _playerStateController = new PlayerStateSwitcher();
            IdleState = new IdleState(this, _playerStateController, IDLE_STATE);
            WalkState = new WalkState(this, _playerStateController, WALK_STATE);
            RunState = new RunState(this, _playerStateController, RUN_STATE);
        }

        private void Start()
        {
            InitializePlayer();
            _playerStateController.Initialize(IdleState);
        }

        private void FixedUpdate()
        {
            if (!_canMove) return;
            _playerStateController.CurrentState.DoFixedState();
        }

        private void Update()
        {
            _playerStateController.CurrentState.DoState();
        }

        #endregion

        #region Methods
        
        private void InitializePlayer()
        {
            gameObject.name = playerName;
            _canMove = true;
        }

        #endregion
    }
}
