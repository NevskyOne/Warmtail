using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entities.PlayerScripts
{
    public class PlayerMovement : IAbility
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _deceleration = 10f;
    
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private Vector2 _currentVelocity = new Vector2();
        public bool Enabled { get; set; }
    
        [Inject]
        public void Construct(Player player, PlayerInput playerInput)
        {
            _rb = player.Rigidbody;
            playerInput.actions["Move"].performed += OnMove;
        }
    
        private void OnMove(InputAction.CallbackContext context)
        {
            if(Enabled)
                _moveInput = context.ReadValue<Vector2>();
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
