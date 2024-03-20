using System;
using System.Collections;
using System.Collections.Generic;
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

        #region Fields & Property

        [Header("General")]
        [SerializeField] private string playerName;
        [SerializeField] private bool isRight;
        private bool _canMove;

        [Header("Movement")]
        [SerializeField] private SpeedStats walkSpeedStats;
        [SerializeField] private SpeedStats runSpeedStats;
        [SerializeField] private float velPower;

        private float _movementValue;
        private float _currentMoveSpeed;
        private float _currentAcceleration;
        private float _currentDecceleration;
        private Vector2 _playerDirection;

        //-- Const Variable
        private const string IS_MOVE = "isMove";

        [Header("State")]
        private PlayerStateController _playerStateController;
        public IdleState IdleState { get; private set; }
        public WalkState WalkState { get; private set; }
        public RunState RunState { get; private set; }

        [Header("Reference")]
        private Rigidbody2D _playerRb;
        private Animator _playerAnim;
        private PlayerInputHandler _playerInputHandler;

        public Animator PlayerAnim => _playerAnim;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerAnim = GetComponentInChildren<Animator>();
            _playerInputHandler = GetComponentInChildren<PlayerInputHandler>();

            _playerStateController = new PlayerStateController();

            IdleState = GetComponentInChildren<IdleState>();
            WalkState = GetComponentInChildren<WalkState>();
            RunState = GetComponentInChildren<RunState>();

            IdleState.InitializeState(this, _playerStateController, "Idle");
            WalkState.InitializeState(this, _playerStateController, "Walk");
            RunState.InitializeState(this, _playerStateController, "Run");
        }

        private void Start()
        {
            InitializePlayer();
            // _playerStateController.Initialize(IdleState);
        }

        private void FixedUpdate()
        {
            PlayerMove();
            // _playerStateController.CurrentState.DoFixedState();
        }

        private void Update()
        {
            PlayerDirection();
            PlayerAnimation();
            // _playerStateController.CurrentState.DoState();
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializePlayer()
        {
            gameObject.name = playerName;
            _canMove = true;

            _currentMoveSpeed = walkSpeedStats.MoveSpeed;
            _currentAcceleration = walkSpeedStats.Acceleration;
            _currentDecceleration = walkSpeedStats.Decceleration;
        }

        // !-- Core Functionality
        private void PlayerMove()
        {
            if (!_canMove) return;

            _playerDirection = new Vector2( _playerInputHandler.Direction.x, _playerDirection.y);
            _playerDirection.Normalize();

            var targetSpeed = _playerDirection * _currentMoveSpeed;
            var speedDif = targetSpeed.x - _playerRb.velocity.x;
            var accelRate = (Mathf.Abs(targetSpeed.x) > 0.01f) ? _currentAcceleration : _currentAcceleration;
            _movementValue = Mathf.Pow(MathF.Abs(speedDif) * accelRate, velPower) * MathF.Sign(speedDif);
            
            _playerRb.AddForce(_movementValue * Vector2.right);
        }

        private void PlayerAnimation()
        {
            var isPlayerMove = _playerDirection.x != 0 || _movementValue >= 1.1f || _movementValue <= -1.1f;
            _playerAnim.SetBool(IS_MOVE, isPlayerMove);
        }

        private void PlayerDirection()
        {
            if (_playerDirection.x > 0 && !isRight)
            {
                PlayerFlip();
            }
            if (_playerDirection.x < 0 && isRight)
            {
                PlayerFlip();
            }
        }

        private void PlayerFlip()
        {
            isRight = !isRight;
            transform.Rotate(0f, 180f, 0f);
        }


        #endregion
    }
}
