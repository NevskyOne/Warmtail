using UnityEngine;
using UnityEngine.UI;
using Interfaces;
using Systems;

namespace Entities.Puzzle
{
    public class Lever : MonoBehaviour, IWarmable
    {
        [SerializeField] private int _warmCapacity;
        [SerializeField] private float _during;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _enableSprite;
        [SerializeField] private Sprite _disableSprite;

        private int _warm;
        private ResettableTimer _timerWarm;
        private ResettableTimer _timerRest;

        public void Warm()
        {
            _warm -= 1;
            if (_warm <= 0) WarmExplosion();
            else if (_timerWarm != null) _timerWarm.Start();
            else _timerWarm = new ResettableTimer(3, Reset);
        }

        public void WarmExplosion() //Enable lever
        {
            _spriteRenderer.sprite = _enableSprite;
            if (_timerRest != null) _timerRest.Start();
            else _timerRest = new ResettableTimer(_during, Disable);
        }

        public void Reset()
        {
            _warm = _warmCapacity;
        }

        private void Disable()
        {
            _spriteRenderer.sprite = _disableSprite;
            Reset();
        }

    }
}
