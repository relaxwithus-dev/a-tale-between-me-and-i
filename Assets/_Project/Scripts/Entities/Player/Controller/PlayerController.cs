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
        [SerializeField] private Vector2 movementDirection;
        [SerializeField] private bool isRight;

        public Vector2 MovementDirection
        {
            get => movementDirection;
            set => movementDirection = value;
        }
        public bool CanMove { get; private set; }

        public PlayerData PlayerData => playerData;

        // !-- State
        public PlayerStateSwitcher StateSwitcher { get; private set; }

        public IdleState IdleState { get; private set; }
        public MoveState WalkState { get; private set; }
        public MoveState RunState { get; private set; }
        public JumpState JumpState { get; private set; }

        // Reference
        public PlayerInputHandler InputHandler { get; private set; }
        public Animator PlayerAnimator { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // Component
            PlayerAnimator = GetComponentInChildren<Animator>();
            InputHandler = GetComponentInChildren<PlayerInputHandler>();

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
            if (!CanMove) return;
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
            gameObject.name = playerData.PlayerName;
            CanMove = true;
        }

        public void StartMovement()
        {
            CanMove = true;
        }
        
        public void StopMovement()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StateSwitcher.SwitchState(IdleState);
            CanMove = false;
        }

        #endregion
    }
}
