using System;
using Data;
using Data.Player;
using Entities.PlayerScripts;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems
{
    [Serializable]
    public class PlayerMovement : IAbility
    {
        public bool Enabled { get; set; }
        public Action StartAbility { get; set; }
        public Action UsingAbility { get; set; }
        public Action EndAbility { get; set; }
        [field: SerializeReference] public IAbilityVisual Visual { get; set; }
        
        [Header("Movement Settings")]
        [SerializeField] private float _moveForce = 100f;

        [SerializeField] private float _moreForge = 100f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _drag = 5f;

        private Rigidbody2D _mainRigidbody;
        private Vector2 _moveInput;
        private GlobalData _globalData;
        
        [Inject]
        public void Construct(Player player, PlayerInput playerInput, GlobalData data)
        {
            _globalData = data;

            _mainRigidbody = player.Rigidbody;

            _mainRigidbody.angularDamping = _drag;
            _mainRigidbody.linearDamping = _drag;

            if (playerInput != null && playerInput.actions != null && playerInput.actions["Move"] != null)
            {
                var moveAction = playerInput.actions["Move"];
                moveAction.started += _ => { if (Enabled) StartAbility?.Invoke(); };
                moveAction.performed += OnMove;
                moveAction.canceled += OnMoveCanceled;
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (Enabled)
            {
                _moveInput = context.ReadValue<Vector2>();
            }
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _moveInput = Vector2.zero;
            EndAbility?.Invoke();
        }

        public void FixedTick()
        {
            if (_mainRigidbody == null || !Enabled) return;
            
            if (_moveInput.magnitude > 0.1f)
            {
                UsingAbility?.Invoke();
                Vector2 force = _moveInput.normalized * _moveForce;
                _mainRigidbody.AddForce(force * _moreForge, ForceMode2D.Force);
                
                float targetAngle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
                float newAngle = Mathf.LerpAngle(_mainRigidbody.rotation, targetAngle, 1.5f * Time.fixedDeltaTime);
                _mainRigidbody.MoveRotation(newAngle);
            }
        }
    }
}