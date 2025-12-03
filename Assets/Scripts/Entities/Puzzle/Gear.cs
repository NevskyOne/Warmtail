using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using Interfaces;
using Systems;

namespace Entities.Puzzle
{
    public class Gear : MonoBehaviour, IWarmable
    {
        [SerializeField] private int _warmCapacity;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _twistingSprite;
        [SerializeField] private Sprite _untwistSprite;

        private int _warm;
        private int _gearId;
        private ResettableTimer _timerWarm;

        public static UnityEvent<int> OnTwisted = new();

        public void Initialize(int id)
        {
            _gearId = id;
            Reset();
            GearsPuzzle.OnReseted.AddListener(Reset);
        }

        public void Warm()
        {
            if (_warm <= 0) return;
            _warm -= 1;
            if (_warm <= 0) WarmExplosion();
            else if (_timerWarm != null) _timerWarm.Start();
            else _timerWarm = new ResettableTimer(_warmCapacity, WarmLost);
        }

        public void WarmExplosion() // twisting
        {
            _spriteRenderer.sprite = _twistingSprite;
            OnTwisted.Invoke(_gearId);
        }

        private void WarmLost() // untwist
        {
            if (_warm > 0) Reset();
        }

        public void Reset() // untwist
        {
            _warm = _warmCapacity;
            _spriteRenderer.sprite = _untwistSprite;
        }
    }
}
