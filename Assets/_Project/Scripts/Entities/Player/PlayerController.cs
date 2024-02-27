using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Property

        [Header("Data")]
        [SerializeField] private string playerName;
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool isRight;

        private Vector2 _playerDirection;
        private bool _canMove;

        //-- Const Variable
        private const string IS_MOVE = "isMove";

        [Header("Reference")]
        private Rigidbody2D _playerRb;
        private Animator _playerAnim;
        private PlayerInputHandler _playerInputHandler;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerAnim = GetComponentInChildren<Animator>();
            _playerInputHandler = GetComponentInChildren<PlayerInputHandler>();
        }

        private void Start()
        {
            InitializePlayer();
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void Update()
        {
            PlayerDirection();
            // PlayerAnimation();
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializePlayer()
        {
            gameObject.name = playerName;
            _canMove = true;
        }

        // !-- Core Functionality
        private void PlayerMove()
        {
            if (!_canMove) return;

            _playerDirection = new Vector2( _playerInputHandler.Direction.x, _playerDirection.y);
            _playerDirection.Normalize();

            _playerRb.velocity = _playerDirection * moveSpeed;
        }

        private void PlayerAnimation()
        {
            var isPlayerMove = _playerDirection != Vector2.zero;
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
            transform.Rotate(0f, 90f, 0f);
        }


        #endregion
    }
}
