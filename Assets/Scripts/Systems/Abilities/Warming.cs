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
    public class WarmingAbility : BaseAbility
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

            StartAbility += StartWarm;
            EndAbility += StopWarm;
        }

        private void StartWarm()
        {
            ActiveRoutine().Forget();
            _canActivate = false;
        }
        
        private async void StopWarm()
        {
            _isRunning = false;
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
            }
            else
            {
                Debug.Log("warm");
                while (Enabled && _isRunning)
                {
                    PerformTick();
                    await UniTask.Delay(500);
                }
            }
        }

        private async UniTask PerformExplosion()
        {
            if (!_warmthSystem.CheckWarmCost(_explosionCost))
            { 
                StopWarm();
                return;
            }
            _warmthSystem.DecreaseWarmth(_explosionCost);
            float timer = 0;
            
            while (timer < _explosionDuration)
            {
                timer += Time.deltaTime;
                float currentRadius = Mathf.Lerp(0, _explosionMaxRadius, timer / _explosionDuration);
                
                var hits = Physics2D.OverlapCircleAll(_playerTransform.position, currentRadius);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Warmable>(out var warmable))
                        warmable.WarmComplete();
                }
                await UniTask.Yield();
            }

            StopWarm();
        }

        private void PerformTick()
        {
            if (!_warmthSystem.CheckWarmCost(_cost))
            {
                StopWarm();
                return;
            }
            var hits = Physics2D.OverlapCircleAll(_playerTransform.position, _radius);
            bool success = false;
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Warmable>(out var warmable))
                {
                    warmable.Warm();
                    success = true;
                }
            }

            if (success)
            {
                _warmthSystem.DecreaseWarmth(_cost);
                UsingAbility?.Invoke();
            }
         
        }
    }
}
