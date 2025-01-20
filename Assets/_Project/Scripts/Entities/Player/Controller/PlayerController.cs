using System;
using System.Collections;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Properties

        /// <Note>
        /// Latest move value.
        /// Walk: speed (2.62), decel (0.185)
        /// Run: speed (3.5), decel (0.37)
        /// </Note>

        [Header("Stats")]
        [SerializeField] private PlayerState playerState = PlayerState.Idle;
        [SerializeField] private PlayerData[] playerDatas;
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private bool isRight;
        [SerializeField] private bool canMove = true;

        private Vector2 _latestDirection;
        private float _currentDecelTime;

        public bool IsRight => isRight;
        public bool CanMove => canMove;
        public Vector2 MoveDirection => moveDirection;
        public PlayerState PlayerState => playerState;

        public PlayerData CurrentData { get; private set; }
        public float CurrentSpeed { get; set; }

        // Reference
        private Rigidbody2D _playerRb;
        private SpriteRenderer _playerSr;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerSr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            InitPlayer();
        }

        private void FixedUpdate()
        {
            if (!CanMove) return;
            PlayerMove();
        }

        private void Update()
        {            
            HandleState();
            PlayerDirection();
        }

        #endregion

        #region Methods
        
        // Initialize
        private void InitPlayer()
        {
            canMove = true;
            CurrentData = playerDatas[0];
            CurrentSpeed = CurrentData.MoveSpeed;
            gameObject.name = CurrentData.PlayerName;
        }
        
        // Core
        private void PlayerMove()
        {
            var direction = GameInputHandler.Instance.MoveDirection;
            moveDirection = new(direction.x, moveDirection.y);
            moveDirection.Normalize();

            if (moveDirection.sqrMagnitude > 0f)
            {
                _playerRb.velocity = moveDirection * CurrentData.MoveSpeed;
                _latestDirection = moveDirection;
                _currentDecelTime = 0f;
            }
            else
            {
                if (_currentDecelTime == 0)
                    StartCoroutine(DeceleratePlayer());
            }
        }

        private IEnumerator DeceleratePlayer()
        {
            var data = CurrentData;

            _currentDecelTime = data.Deceleration;
            while (_currentDecelTime > 0f)
            {
                var deccelSpeed = Mathf.Lerp(data.MoveSpeed, 0f, 1f - (_currentDecelTime / data.Deceleration));
                _playerRb.velocity = _latestDirection * deccelSpeed;
                _currentDecelTime -= Time.deltaTime;
                yield return null;
            }

            _playerRb.velocity = Vector2.zero; 
            _latestDirection = Vector2.zero;
        }

        private void PlayerDirection()
        {
            var direction = moveDirection;
            if (direction.x > 0 && !isRight || direction.x < 0 && isRight)
                PlayerFlip();
        }

        // TODO: Uncomment flip dan drop rotate misal sprite aman
        public void PlayerFlip()
        {
            isRight = !isRight;
            // _playerSr.flipX = isRight;
            _playerSr.transform.Rotate(0f, 180f, 0f);
        }

        public void StartMovement()
        {
            canMove = true;
        }

        public void StopMovement()
        {
            canMove = false;
            moveDirection = Vector2.zero;
            _latestDirection = Vector2.zero;
            _playerRb.velocity = Vector2.zero;
        }

        #endregion

        #region State

        private void HandleState()
        {
            var state = GetState();

            if (playerState == state) return;
            playerState = state;
            CurrentData = GetCurrentData(state);
            CurrentSpeed = CurrentData.MoveSpeed;
        }

        private PlayerState GetState()
        {
            var direction = MoveDirection;
            var isRunning = GameInputHandler.Instance.IsPressRun;

            if (direction == Vector2.zero) return PlayerState.Idle;
            return isRunning ? PlayerState.Run : PlayerState.Walk;
        }
        
        private PlayerData GetCurrentData(PlayerState state)
        {
            return state switch
            {
                PlayerState.Idle => CurrentData,
                PlayerState.Walk => playerDatas[0],
                PlayerState.Run => playerDatas[1],
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }

        #endregion
    }
}