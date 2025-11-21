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
        [Header("Movement Settings")]
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _deceleration = 10f;
    
        private float _moveSpeed = 5f;
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private Vector2 _currentVelocity;
        private GlobalData _globalData;
        public bool Enabled { get; set; }
        public Action StartAbility { get; set; }
        public Action UsingAbility { get; set; }
        public Action EndAbility { get; set; }


        [Inject]
        public void Construct(Player player, PlayerInput playerInput, GlobalData data)
        {
            _rb = player.Rigidbody;
            _globalData = data;
            GetNewSpeed();
            _globalData.SubscribeTo<RuntimePlayerData>(GetNewSpeed);
            playerInput.actions["Move"].started += _ => StartAbility?.Invoke();
            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }

        private void GetNewSpeed()
        {
            _moveSpeed = _globalData.Get<RuntimePlayerData>().Speed;
        }
    
        private void OnMove(InputAction.CallbackContext context)
        {
            if (Enabled)
            {
                _moveInput = context.ReadValue<Vector2>();
                UsingAbility?.Invoke();
            }
            else{
                _moveInput = Vector2.zero;
            }
        }
    
        public void FixedTick()
        {
            if (_moveInput.magnitude > 0.1f)
            {
                _currentVelocity = Vector2.MoveTowards(
                    _currentVelocity, 
                    _moveInput.normalized * _moveSpeed, 
                    _acceleration * Time.fixedDeltaTime
                );
            }
            else
            {
                _currentVelocity = Vector2.MoveTowards(
                    _currentVelocity, 
                    Vector2.zero, 
                    _deceleration * Time.fixedDeltaTime
                );
            }
        
            _rb.linearVelocity = _currentVelocity;
        }
    }
}
