using UnityEngine;
using UnityEngine.UI;
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
        private ResettableTimer _timerWarm;

        public void Warm()
        {
            _warm -= 1;
            if (_warm <= 0) WarmExplosion();
            else if (_timerWarm != null) _timerWarm.Start();
            else _timerWarm = new ResettableTimer(3, Reset);
        }

        public void WarmExplosion() // twisting
        {
            _spriteRenderer.sprite = _twistingSprite;
        }

        public void Reset() // untwist
        {
            _warm = _warmCapacity;
            _spriteRenderer.sprite = _untwistSprite;
        }
    }
}
