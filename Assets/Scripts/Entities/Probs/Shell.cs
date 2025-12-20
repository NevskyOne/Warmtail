using Data;
using Data.Player;
using Interfaces;
using PrimeTween;
using Systems;
using UnityEngine;
using Zenject;

namespace Entities.Probs
{
    public class Shell : MonoBehaviour, IWarmable
    {
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        [SerializeField] private int _warmCapacity;
        [SerializeField] private int _shellsAmount;
        private GlobalData _globalData;
        private int _warm;
        private ResettableTimer _timer;
        private MaterialPropertyBlock _propertyBlock;
        private SpriteRenderer _renderer;
        private Tween? _tween;
    
        [Inject]
        public void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            DailySystem.OnLoadedResources += LoadShell;
            DailySystem.OnDiscardedResources += DiscardShell;
        }

        private void Start()
        {
            _propertyBlock = new();
            _renderer = GetComponent<SpriteRenderer>();
            Reset();
            
        }

        private void OnDestroy()
        {
            DailySystem.OnLoadedResources -= LoadShell;
            DailySystem.OnDiscardedResources -= DiscardShell;
        }
        
        public void Warm()
        {
            UpdateRenderer((_warmCapacity - _warm) * 1.0f / _warmCapacity,
                (_warmCapacity - _warm - 1) * 1.0f / _warmCapacity);
            _warm -= 1;
            if (_warm == 0)
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
            _globalData.Edit<SavablePlayerData>((playerData) =>
            {
                playerData.Shells += _shellsAmount;
            });
            _globalData.Edit<ShellsData>(data => {
                data.ShellsActive[ConvertFloatsToString (transform.position.x, transform.position.y)] = false;
            });
            _timer.Stop();
            Destroy(gameObject);
        }
        
        public void Reset()
        {
            UpdateRenderer((_warmCapacity - _warm) * 1.0f / _warmCapacity, 0);
            _warm = _warmCapacity;
        }

        private void LoadShell()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            CheckShellData(x, y);
            if (!_globalData.Get<ShellsData>().ShellsActive[ConvertFloatsToString(x, y)])
                Destroy(gameObject);
        }

        private void DiscardShell()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            CheckShellData(x, y);
            _globalData.Edit<ShellsData>(data => data.ShellsActive[ConvertFloatsToString(x, y)] = true);
        }

        private void CheckShellData(float x, float y)
        {
            if (!_globalData.Get<ShellsData>().ShellsActive.ContainsKey(ConvertFloatsToString(x, y)))
                _globalData.Edit<ShellsData>(data => data.ShellsActive[ConvertFloatsToString(x, y)] = true);
        }

        private string ConvertFloatsToString(float x, float y)
        {
            return (x + " : " + y);
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
                _propertyBlock.SetFloat(DissolveAmount, x);
                _renderer.SetPropertyBlock(_propertyBlock);
            });
            await _tween.Value;
        }
    }
}
