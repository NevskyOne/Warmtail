using System;
using Cysharp.Threading.Tasks;
using Entities.PlayerScripts;
using Interfaces;
using Systems.Abilities;
using UnityEngine;
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

        [Inject]
        public void Construct(Player player, WarmthSystem warmth)
        {
            _playerTransform = player.transform;
            _warmthSystem = warmth;
            StartAbility += () => ActiveRoutine().Forget();
        }

        private async UniTaskVoid ActiveRoutine()
        {
            if (IsComboActive && _secondaryComboType == typeof(MetabolismAbility))
            {
                await PerformExplosion();
                EndAbility?.Invoke(); // Авто-завершение после взрыва
            }
            else
            {
                while (Enabled)
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
    }
}
