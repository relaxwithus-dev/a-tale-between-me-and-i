using System;
using System.Collections;
using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Const Variable
        private const string IDLE_STATE = "Idle";
        private const string WALK_STATE = "Walk";
        private const string RUN_STATE = "Run";
        private const string JUMP_STATE = "Jump";

        private const string IS_MOVE = "isMove";
        #endregion

        #region Fields & Property

        [Header("Data")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private string playerName;
        [SerializeField] private bool isRight;

        private bool _canMove;

        public PlayerData PlayerData => playerData;

        // !-- State
        public PlayerStateSwitcher StateSwitcher { get; private set; }

        public IdleState IdleState { get; private set; }
        public MoveState WalkState { get; private set; }
        public MoveState RunState { get; private set; }
        public JumpState JumpState { get; private set; }

        // !-- Reference
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
            StateSwitcher = new PlayerStateSwitcher();
            IdleState = new IdleState(this, StateSwitcher, IDLE_STATE);
            WalkState = new MoveState(this, StateSwitcher, WALK_STATE);
            RunState = new MoveState(this, StateSwitcher, RUN_STATE);
        }

        private void Start()
        {
            InitializePlayer();
            StateSwitcher.Initialize(IdleState);
        }

        private void FixedUpdate()
        {
            if (!_canMove) return;
            StateSwitcher.CurrentState.DoFixedState();
        }

        private void Update()
        {
            StateSwitcher.CurrentState.DoState();
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
