using System.Collections;
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
        
        public IEnumerator StartDrain()
        {
            if(_isFreezing) yield break;
            _isFreezing = true;
            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1);
            var counter = 0f;
            
            while (_currentFreeze < 1)
            {
                yield return new WaitForSeconds(0.2f);
                _currentFreeze = Mathf.Pow(10,counter)/10;
                _freezeMaterial.SetFloat(DissolveAmount, 1 - _currentFreeze);
                
                counter += _freezeRate;
                Debug.Log("Drain");
            }
            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1);
            _player.Die();
        }
        
        public IEnumerator StopDrain()
        {
            _isFreezing = false;
            Debug.Log("Stop Drain");
            while (_currentFreeze > 0)
            {
                yield return new WaitForSeconds(0.2f);
                _currentFreeze -= _freezeRate;
                _freezeMaterial.SetFloat(DissolveAmount, 1 - _currentFreeze);
            }

            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1 - _currentFreeze);
            Debug.Log("Freeze = 0");
        }
        
        private void OnDisable()
        {
            _currentFreeze = 0;
            _freezeMaterial.SetFloat(DissolveAmount, 1);
        }
    }
}