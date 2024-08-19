using System;
using System.Collections;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Enum;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Player
{
    /// <summary>
    /// PlayerController buat handle controlling
    /// karakter player, termasuk navigasi, movement,
    /// dan lainnya.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Properties

        /// <Note>
        /// Early move value.
        /// Walk: speed (2.3), decel (0.18)
        /// Run: speed (3.16), decel (0.235)
        /// </Note>

        [Header("Stats")]
        [SerializeField] private PlayerState playerState = PlayerState.Idle;
        [SerializeField] private PlayerData[] playerDatas;
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private bool isRight;
        [SerializeField] private bool canMove = true;

        private PlayerData _currentData;
        private Vector2 _latestDirection;
        private float _currentDecelTime;

        public bool IsRight => isRight;
        public bool CanMove => canMove;
        public Vector2 MoveDirection => moveDirection;
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
        
        // !- Initialize
        private void InitPlayer()
        {
            canMove = true;
            _currentData = playerDatas[0];
            CurrentSpeed = _currentData.MoveSpeed;
            gameObject.name = _currentData.PlayerName;
        }
        
        // !- Core
        private void PlayerMove()
        {
            var direction = GameInputHandler.Instance.MoveDirection;
            moveDirection = new(direction.x, moveDirection.y);
            moveDirection.Normalize();

            if (moveDirection.sqrMagnitude > 0f)
            {
                _playerRb.velocity = moveDirection * _currentData.MoveSpeed;
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
            var data = _currentData;

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
            transform.Rotate(0f, 180f, 0f);
        }

        // !- Helpers
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
            _currentData = GetCurrentData(state);
            CurrentSpeed = _currentData.MoveSpeed;
        }

        private PlayerState GetState()
        {
            var direction = MoveDirection;

            if (direction != Vector2.zero && GameInputHandler.Instance.IsPressRun) return PlayerState.Run;
            if (direction != Vector2.zero && !GameInputHandler.Instance.IsPressRun) return PlayerState.Walk;
            return PlayerState.Idle;
            // return direction != Vector2.zero ? PlayerState.Walk : PlayerState.Idle;
        }
        
        private PlayerData GetCurrentData(PlayerState playerState)
        {
            return playerState switch
            {
                PlayerState.Idle => _currentData,
                PlayerState.Walk => playerDatas[0],
                PlayerState.Run => playerDatas[1],
                _ => throw new ArgumentOutOfRangeException(nameof(playerState), playerState, null)
            };
        }

        #endregion
    }
}