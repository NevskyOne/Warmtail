using System;
using Entities.PlayerScripts;
using Interfaces;
using Systems.Abilities;
using Systems.Environment;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.Abilities.Concrete
{
    [Serializable]
    public class DashAbility : BaseAbility
    {
        [SerializeField] private float _dashForce = 25f;
        [SerializeField] private int _dashCost = 15;
        [SerializeField] private float _destroyRadius = 1.5f;

        private Rigidbody2D _playerRb;
        private SurfacingSystem _surfacingSystem;
        private WarmthSystem _warmthSystem;
        
        private Vector2 _moveInput;
        private float _layerInput; // -1, 0, 1

        [Inject]
        public void Construct(
            Player player,
            WarmthSystem warmth,
            SurfacingSystem surfacing,
            PlayerInput input,
            DiContainer container)
        {
            _playerRb = player.Rigidbody;
            _surfacingSystem = surfacing;
            _warmthSystem = warmth;

            input.actions["Move"].performed += ctx => _moveInput = ctx.ReadValue<Vector2>();

            input.actions["Surfacing"].performed += ctx => _layerInput = ctx.ReadValue<float>();
            input.actions["Surfacing"].canceled += _ => _layerInput = 0;

            UsingAbility += OnTick;
        }


        private void OnTick()
        {
            if (!Enabled) return;

            bool isFree = IsComboActive && _secondaryComboType == typeof(MetabolismAbility);

            // Логика смены слоя (всплытие/погружение)
            if (Mathf.Abs(_layerInput) > 0.1f)
            {
                int dir = (int)Mathf.Sign(_layerInput);
                if (_surfacingSystem.TryChangeLayer(dir))
                {
                    HandleObstacleDestruction();
                    if (!isFree) _warmthSystem.DecreaseWarmth(_dashCost);
                    
                    // Сбрасываем инпут, чтобы не пролетать слои мгновенно
                    _layerInput = 0; 
                }
            }
            // Логика рывка
            else if (_moveInput.magnitude > 0.1f)
            {
                _playerRb.AddForce(_moveInput * _dashForce, ForceMode2D.Force);
                if (!isFree) _warmthSystem.DecreaseWarmth(_dashCost);
            }
        }

        private void HandleObstacleDestruction()
        {
            var hits = Physics2D.OverlapCircleAll(_playerRb.position, _destroyRadius);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDestroyable>(out var dest))
                    dest.DestroyObject();
            }
        }
    }
}
