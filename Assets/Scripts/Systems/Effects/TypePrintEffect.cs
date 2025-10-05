using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Interfaces;
using TMPro;
using UnityEngine;

namespace Systems.Effects
{
    public class TypePrintEffect : IPrintEffect
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _fadeDuration = 0.05f;
        private bool _speedUp;

        public async Task<bool> StartEffect(TMP_Text text)
        {
            _speedUp = false;
            text.maxVisibleCharacters = 0;
            text.alpha = 1;
            text.ForceMeshUpdate();

            for (int i = 0; i < text.text.Length; i++)
            {
                text.maxVisibleCharacters = i + 1;
                await UniTask.Delay((int)(_speedUp ? _fadeDuration * 200 : 1000f / _speed));
            }

            return true;
        }

        public void SpeedUpEffect()
        {
            _speedUp = true;
        }
    }
}