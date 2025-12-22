using Interfaces;
using PrimeTween;
using Systems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

namespace Entities.Probs
{
    public class WarmableIce : Warmable
    {
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        [SerializeField] private SpriteShapeRenderer _renderer;
        
        private ResettableTimer _timer;
        private Tween? _tween;
        private MaterialPropertyBlock _propertyBlock;

        private void Start()
        {
            _propertyBlock = new();
            Reset();
        }
        
        public override void Warm()
        {
            UpdateRenderer((_maxWarmthAmount - _warmthAmount) * 1.0f / _maxWarmthAmount,
                (_maxWarmthAmount - _warmthAmount - _warmFactor) * 1.0f / _maxWarmthAmount);
            base.Warm();
            if(_warmthAmount > 0)
            {
                if (_timer != null)
                    _timer.Start();
                else
                    _timer = new ResettableTimer(3, Reset);
            }
        }

        public override void WarmComplete()
        {
            _timer.Stop();
            Destroy(gameObject);
        }

        public override void Reset()
        {
            UpdateRenderer((_maxWarmthAmount - _warmthAmount) * 1.0f / _maxWarmthAmount, 0);
            base.Reset();
        }
        
        private async void UpdateRenderer(float lastAmount, float newAmount)
        {
            if (!_renderer) return;
            _tween?.Stop();
            _tween = Tween.Custom(lastAmount, newAmount, 0.3f, x =>
            {
                if (!_renderer)
                {
                    _tween?.Stop();
                    return;
                }
                _propertyBlock.SetFloat(DissolveAmount, x);
                _renderer.SetPropertyBlock(_propertyBlock);
            });
            await _tween.Value;
        }
    }
}