using System;
using System.Collections;
using ATBMI.Cutscene;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Entities.Player
{
    public class PlayerController : MonoBehaviour, IController
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private EntitiesState playerState = EntitiesState.Idle;
        [SerializeField] private PlayerData[] playerData;
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private bool isFacingRight;
        [SerializeField] private bool canMove;

        private float _currentDeceleration;
        private Vector2 _latestDirection;
        private Vector2 _temporaryDirection;
        private PlayerData _currentData;
        
        public PlayerData Data => _currentData;
        public bool IsFacingRight => isFacingRight;
        public bool CanMove => canMove;
        public Vector2 MoveDirection => moveDirection;
        public EntitiesState PlayerState => playerState;

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

        private void OnEnable()
        {
            InitPlayer();
        }
        
        private void FixedUpdate()
        {
            if (!CanMove || DialogueManager.Instance.IsDialoguePlaying 
                         || CutsceneManager.Instance.IsCutscenePlaying) return;
            PlayerMove();
        }
        
        private void Update()
        {
            if (DialogueManager.Instance.IsDialoguePlaying || CutsceneManager.Instance.IsCutscenePlaying) return;
            HandleState();
            LookAt(moveDirection);
        }
        
        // Initialize
        public void InitPlayer(string playerName = "Dewa")
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
        
        public void LookAt(Vector2 direction)
        {
            if (direction.x > 0 && !isFacingRight || direction.x < 0 && isFacingRight)
                Flip();
        }
        
        public void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        
        // Helpers
        public void StartMovement()
        {
            canMove = true;
        }

        public void StopMovement()
        {
            canMove = false;
            playerState = EntitiesState.Idle;
            
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
        
        public void ChangeState(EntitiesState state)
        {
            if (state == playerState) 
                return;
            
            playerState = state;
        }
        
        private void HandleState()
        {
            var state = GetState();

            DialogueEvents.PlayerRunEvent(state == EntitiesState.Run);

            if (playerState == state) return;
            playerState = state;
            CurrentStat = GetMoveStats(state);
            CurrentSpeed = CurrentStat.Speed;
        }
        
        private EntitiesState GetState()
        {
            var direction = MoveDirection;
            var isRunning = GameInputHandler.Instance.IsPressRun;

            if (direction == Vector2.zero) return EntitiesState.Idle;
            return isRunning ? EntitiesState.Run : EntitiesState.Walk;
        }
        
        private PlayerData.MoveStat GetMoveStats(EntitiesState state)
        {
            if (state == EntitiesState.Idle)
                return CurrentStat;

            return Array.Find(_currentData.MoveStats, stat => stat.State == state);
        }

        #endregion
    }
}