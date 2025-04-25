using System;
using System.Collections;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private PlayerState playerState = PlayerState.Idle;
        [SerializeField] private PlayerData[] playerData;
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private bool isRight;
        [SerializeField] private bool canMove;

        private float _currentDeceleration;
        private Vector2 _latestDirection;
        private Vector2 _temporaryDirection;
        private PlayerData _currentData;
        
        public PlayerData Data => _currentData;
        public bool IsRight => isRight;
        public bool CanMove => canMove;
        public Vector2 MoveDirection => moveDirection;
        public PlayerState PlayerState => playerState;

        public PlayerData.MoveStat CurrentStat { get; private set; }
        public float CurrentSpeed { get; set; }

        // Reference
        private SpriteRenderer _playerSr;
        private Animator _playerAnimator;
        private PlayerAnimation _playerAnimation;
        public Rigidbody2D PlayerRb { get; private set; }

        #endregion

        #region Methods

        // Unity Callbacks
        private void Awake()
        {
            PlayerRb = GetComponent<Rigidbody2D>();
            
            _playerSr = GetComponentInChildren<SpriteRenderer>();
            _playerAnimator = GetComponentInChildren<Animator>();
            _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        }

        private void Start()
        {
            InitPlayer();
        }

        private void FixedUpdate()
        {
            if (!CanMove || DialogueManager.Instance.IsDialoguePlaying) return;
            PlayerMove();
        }

        private void Update()
        {
            HandleState();
            PlayerDirection();
        }
        
        // Initialize
        private void InitPlayer(string playerName = "Dewa")
        {
            _currentData = Array.Find(playerData, data => data.PlayerName == playerName);
            if (_currentData == null)
            {
                Debug.LogWarning("player target data not found!");
                return;
            }
            
            // Stats
            gameObject.name = _currentData.PlayerName;
            CurrentStat = _currentData.MoveStats[0];
            CurrentSpeed = CurrentStat.Speed;
            
            // Assets
            _playerSr.sprite = _currentData.PlayerSprite;
            _playerAnimator.runtimeAnimatorController = _currentData.PlayerAnimator;
            _playerAnimation.InitAnimationHash();
        }

        // Core
        private void PlayerMove()
        {
            var direction = _temporaryDirection == Vector2.zero
                ? GameInputHandler.Instance.MoveDirection
                : _temporaryDirection;

            moveDirection = new Vector2(direction.x, moveDirection.y);
            moveDirection.Normalize();

            if (moveDirection.sqrMagnitude > 0f)
            {
                PlayerRb.velocity = moveDirection * CurrentStat.Speed;
                _latestDirection = moveDirection;
                _currentDeceleration = 0f;
            }
            else
            {
                if (_currentDeceleration == 0)
                    StartCoroutine(DeceleratePlayer());
            }
        }

        private IEnumerator DeceleratePlayer()
        {
            var data = CurrentStat;
            _currentDeceleration = data.Deceleration;

            while (_currentDeceleration > 0f)
            {
                var decelSpeed = Mathf.Lerp(data.Speed, 0f, 1f - (_currentDeceleration / data.Deceleration));
                PlayerRb.velocity = _latestDirection * decelSpeed;
                _currentDeceleration -= Time.deltaTime;
                yield return null;
            }

            PlayerRb.velocity = Vector2.zero;
            _latestDirection = Vector2.zero;
        }

        private void PlayerDirection()
        {
            var direction = moveDirection;
            if (direction.x > 0 && !isRight || direction.x < 0 && isRight)
                PlayerFlip();
        }

        public void PlayerFlip()
        {
            isRight = !isRight;
            _playerSr.flipX = !isRight;
        }

        // Helpers
        public void StartMovement()
        {
            canMove = true;
        }

        public void StopMovement()
        {
            canMove = false;
            moveDirection = Vector2.zero;
            _latestDirection = Vector2.zero;
            PlayerRb.velocity = Vector2.zero;
        }

        public void SetTemporaryDirection(Vector2 direction)
        {
            _temporaryDirection = direction;
        }

        #endregion

        #region State

        private void HandleState()
        {
            var state = GetState();

            DialogueEvents.PlayerRunEvent(state == PlayerState.Run);

            if (playerState == state) return;
            playerState = state;
            CurrentStat = GetMoveStats(state);
            CurrentSpeed = CurrentStat.Speed;
        }

        private PlayerState GetState()
        {
            var direction = MoveDirection;
            var isRunning = GameInputHandler.Instance.IsPressRun;

            if (direction == Vector2.zero) return PlayerState.Idle;
            return isRunning ? PlayerState.Run : PlayerState.Walk;
        }

        private PlayerData.MoveStat GetMoveStats(PlayerState state)
        {
            if (state == PlayerState.Idle)
                return CurrentStat;

            return Array.Find(_currentData.MoveStats, stat => stat.State == state);
        }

        #endregion
    }
}