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
        [SerializeField] private string playerName;
        [SerializeField] private PlayerData[] playerData;
        [SerializeField] private Vector2 movementDirection;
        [SerializeField] private bool isRight;

        public bool IsRight => isRight;
        [field: SerializeField] public float CurrentSpeed { get; set; }
        public Vector2 MovementDirection
        {
            get => movementDirection;
            set => movementDirection = value;
        }
        public bool CanMove { get; private set; }
        public PlayerData[] PlayerData => playerData;

        // !-- State
        public PlayerStateSwitcher StateSwitcher { get; private set; }

        public IdleState IdleState { get; private set; }
        public MoveState WalkState { get; private set; }
        public MoveState RunState { get; private set; }
        public JumpState JumpState { get; private set; }

        // !-- Reference
        private TextAsset INKJson;
        private PlayerInputHandler _playerInputHandler;
        private Animator _playerAnimator;
        public PlayerInputHandler PlayerInputHandler => _playerInputHandler;
        public Animator PlayerAnimator => _playerAnimator;

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
            WalkState = new MoveState(this, playerData[0], StateSwitcher, WALK_STATE);
            RunState = new MoveState(this, playerData[1], StateSwitcher, RUN_STATE);
        }

        private void Start()
        {
            InitializePlayer();
            CurrentSpeed = playerData[0].MoveSpeed;
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

        private IEnumerator Move(float newPositionX, bool isNpcFacingRight)
        {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = new Vector3(newPositionX, initialPosition.y, initialPosition.z);

            // Calculate the distance manually
            float distance = Mathf.Abs(targetPosition.x - initialPosition.x);
            float speed = (playerData.RunStats.MoveSpeed / 2) / distance; // Speed is inversely proportional to distance

            float progress = 0f;

            while (progress <= 1f)
            {
                progress += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
                yield return null;
            }

            // Ensure the final position is exactly newPosition
            transform.position = targetPosition;

            if (FlipPlayerWhenDialogue(newPositionX, isNpcFacingRight))
            {
                yield return new WaitForSeconds(0.2f);
            }

            EventHandler.CallEnterDialogueEvent(INKJson);
            INKJson = null;
        }

        private bool FlipPlayerWhenDialogue(float newPositionX, bool isNpcFacingRight)
        {
            if (transform.position.x < newPositionX)
            {
                if (!playerData.IsRight && !isNpcFacingRight)
                {
                    StateSwitcher.CurrentState.PlayerFlip();
                    return true;
                }
                else if (!playerData.IsRight && isNpcFacingRight)
                {
                    StateSwitcher.CurrentState.PlayerFlip();
                    return true;
                }
            }
            else if (transform.position.x > newPositionX)
            {
                if (playerData.IsRight && !isNpcFacingRight)
                {
                    StateSwitcher.CurrentState.PlayerFlip();
                    return true;
                }
                else if (playerData.IsRight && isNpcFacingRight)
                {
                    StateSwitcher.CurrentState.PlayerFlip();
                    return true;
                }
            }
            else if (transform.position.x == newPositionX)
            {
                if (!playerData.IsRight && !isNpcFacingRight)
                {
                    StateSwitcher.CurrentState.PlayerFlip();
                    return true;
                }
                else if (playerData.IsRight && isNpcFacingRight)
                {
                    StateSwitcher.CurrentState.PlayerFlip();
                    return true;
                }
            }

            return false;
        }

        private void InitializePlayer()
        {
            gameObject.name = playerName;
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

        public void MoveToDialogueEntryPoint(TextAsset INKJson, float newPositionX, bool isNpcFacingRight)
        {
            this.INKJson = INKJson;

            FlipPlayerWhenDialogue(newPositionX, isNpcFacingRight);

            // TODO: change to dotween?
            StartCoroutine(Move(newPositionX, isNpcFacingRight));
        }

        #endregion
    }
}
