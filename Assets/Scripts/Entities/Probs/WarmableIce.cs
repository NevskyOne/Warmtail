using Interfaces;
using PrimeTween;
using Systems;
using UnityEngine;
using UnityEngine.U2D;

namespace Entities.Probs
{
    public class WarmableIce : MonoBehaviour, IWarmable
    {
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        [SerializeField] private SpriteShapeRenderer _renderer;
        [SerializeField] private int _warmCapacity;
        private int _warm;
        private ResettableTimer _timer;
        private Tween? _tween;
        private MaterialPropertyBlock _propertyBlock;

        private void Start()
        {
            _propertyBlock = new();
            Reset();
        }
        
        public void Warm()
        {
            UpdateRenderer((_warmCapacity - _warm) * 1.0f / _warmCapacity,
                (_warmCapacity - _warm - 1) * 1.0f / _warmCapacity);
            _warm -= 1;
            if (_warm <= 0)
            {
                WarmExplosion();
            }
            else
            {
                if (_timer != null)
                    _timer.Start();
                else
                    _timer = new ResettableTimer(3, Reset);
            }
        }

        public void WarmExplosion()
        {
            _timer.Stop();
            Destroy(gameObject);
        }

        public void Reset()
        {
            UpdateRenderer((_warmCapacity - _warm) * 1.0f / _warmCapacity, 0);
            _warm = _warmCapacity;
        }
        
        private async void UpdateRenderer(float lastAmount, float newAmount)
        {
            if (!_renderer) return;
            _tween?.Stop();
            _tween = Tween.Custom(lastAmount, newAmount, 0.5f, x =>
            {
                if (!_renderer)
                {
                    _tween?.Stop();
                    return;
                }
                print(x);
                _propertyBlock.SetFloat(DissolveAmount, x);
                _renderer.SetPropertyBlock(_propertyBlock);
            });
            await _tween.Value;
        }
    }
}