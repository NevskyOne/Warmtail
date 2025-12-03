using System.Threading;
using Cysharp.Threading.Tasks;
using Entities.PlayerScripts;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class FreezeVisuals : MonoBehaviour
    {
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        [SerializeField] private Material _freezeMaterial;
        [SerializeField] private float _freezeRate = 0.2f;
        private float _currentFreeze;
        private CancellationTokenSource _token;
        private bool _isFreezing;
        [Inject] private Player _player;
        
        public async UniTaskVoid StartDrain()
        {
            if(_isFreezing) return;
            _isFreezing = true;
            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1);
            var counter = 0f;
            
            _token?.Dispose();
            _token = new CancellationTokenSource();
            while (_currentFreeze < 1)
            {
                await UniTask.Delay(200, cancellationToken: _token.Token);
                _currentFreeze = Mathf.Pow(10,counter)/10;
                _freezeMaterial.SetFloat(DissolveAmount, 1 - _currentFreeze);
                
                counter += _freezeRate;
                Debug.Log("Drain");
            }
            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1);
            _player.Die();
        }
        
        public async UniTaskVoid StopDrain()
        {
            _isFreezing = false;
            _token?.Dispose();
            _token = new CancellationTokenSource();
            Debug.Log("Stop Drain");
            while (_currentFreeze > 0)
            {
                await UniTask.Delay(200, cancellationToken: _token.Token);
                _currentFreeze -= _freezeRate;
                _freezeMaterial.SetFloat(DissolveAmount, 1 - _currentFreeze);
            }
            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1 - _currentFreeze);
            Debug.Log("Freeze = 0");
        }
    }
}