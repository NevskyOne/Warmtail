using System;
using Cysharp.Threading.Tasks;
using Entities.PlayerScripts;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.Abilities.Concrete
{
    [Serializable]
    public class WarmingAbility : BaseAbility, IDisposable
    {
        [Header("Normal Warm")]
        [SerializeField] private float _radius = 3f;
        [SerializeField] private int _cost = 5;
        
        [Header("Explosion")]
        [SerializeField] private float _explosionMaxRadius = 10f;
        [SerializeField] private float _explosionDuration = 0.5f;
        [SerializeField] private int _explosionCost = 50;

        private Transform _playerTransform;
        private WarmthSystem _warmthSystem;
        private PlayerInput _playerInput;
        private bool _isRunning;
        private bool _canActivate;

        [Inject]
        public void Construct(Player player, PlayerInput playerInput, WarmthSystem warmth)
        {
            _playerTransform = player.Rigidbody.transform;
            _warmthSystem = warmth;
            _playerInput = playerInput;
            StartAbility += () => ActiveRoutine().Forget();
            EndAbility += () => _isRunning = false;
            _playerInput.actions["RightMouse"].started += StartWarm;
            _playerInput.actions["RightMouse"].canceled += StopWarm;
        }

        private void StartWarm(InputAction.CallbackContext callbackContext)
        {
            StartAbility?.Invoke();
            _canActivate = false;
        }
        
        private async void StopWarm(InputAction.CallbackContext callbackContext)
        {
            EndAbility?.Invoke();
            await UniTask.Delay(1000);
            _canActivate = true;
        }
        
        private async UniTaskVoid ActiveRoutine()
        {
            if (!_canActivate) return;
            _isRunning = true;
            if (IsComboActive && _secondaryComboType == typeof(MetabolismAbility))
            {
                await PerformExplosion();
                EndAbility?.Invoke(); // Авто-завершение после взрыва
            }
            else
            {
                while (Enabled && _isRunning)
                {
                    PerformTick();
                    await UniTask.Delay(500);
                }
            }
        }

        private async UniTask PerformExplosion()
        {
            _warmthSystem.DecreaseWarmth(_explosionCost);
            float timer = 0;
            
            while (timer < _explosionDuration)
            {
                timer += Time.deltaTime;
                float currentRadius = Mathf.Lerp(0, _explosionMaxRadius, timer / _explosionDuration);
                
                var hits = Physics2D.OverlapCircleAll(_playerTransform.position, currentRadius);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<IWarmable>(out var warmable))
                        warmable.WarmExplosion();
                }
                await UniTask.Yield();
            }
        }

        private void PerformTick()
        {
            var hits = Physics2D.OverlapCircleAll(_playerTransform.position, _radius);
            bool success = false;
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IWarmable>(out var warmable))
                {
                    warmable.Warm();
                    success = true;
                }
            }
            if (success) _warmthSystem.DecreaseWarmth(_cost);
        }

        public void Dispose()
        {
            _playerInput.actions["RightMouse"].started -= StartWarm;
            _playerInput.actions["RightMouse"].canceled -= StopWarm;
        }
    }
}
