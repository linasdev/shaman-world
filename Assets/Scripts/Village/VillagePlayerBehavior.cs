using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Village
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public class VillagePlayerBehavior : MonoBehaviour
    {
        [Range(0.5f, 5f)]
        public float maxHorizontalVelocity;

        [Range(1f, 50f)]
        public float acceleration;

        [Range(0.5f, 5f)]
        public float jumpSpeed;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidBody;
        private float _moveDirection;
        private bool _jumpRequested;
        private InputAction _moveAction;
        private InputAction _jumpAction;

        public void Start()
        {
            var actionMap = InputSystem.actions.FindActionMap("Village");

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _moveAction = actionMap.FindAction("Move");
            _jumpAction = actionMap.FindAction("Jump");
        }

        public void Update()
        {
            _moveDirection = _moveAction.ReadValue<float>();

            if (_jumpAction.triggered)
            {
                _jumpRequested = true;
            }
        }

        public void FixedUpdate()
        {
            HandleMove();
            HandleJump();
            ClampHorizontalVelocity();
        }

        private void HandleMove()
        {
            if (_moveDirection == 0)
            {
                return;
            }

            _rigidBody.AddForceX(_moveDirection * acceleration * _rigidBody.mass);
            _spriteRenderer.flipX = _rigidBody.linearVelocityX < 0;
        }

        private void HandleJump()
        {
            if (_jumpRequested)
            {
                _jumpRequested = false;
                _rigidBody.AddForceY(jumpSpeed * _rigidBody.mass, ForceMode2D.Impulse);
            }
        }

        private void ClampHorizontalVelocity()
        {
            // This doesn't work with friction but we don't need friction anyway
            var nextHorizontalVelocity = _rigidBody.linearVelocityX + _rigidBody.totalForce.x / _rigidBody.mass * Time.fixedDeltaTime;

            if (Mathf.Abs(nextHorizontalVelocity) <= Mathf.Abs(maxHorizontalVelocity))
            {
                return;
            }

            var targetHorizontalVelocity = Mathf.Sign(nextHorizontalVelocity) * maxHorizontalVelocity;
            var horizontalAcceleration = targetHorizontalVelocity - nextHorizontalVelocity;
            _rigidBody.AddForceX(horizontalAcceleration * _rigidBody.mass, ForceMode2D.Impulse);
        }
    }
}
