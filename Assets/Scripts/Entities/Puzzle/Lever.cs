using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
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

        public static UnityEvent OnTurnedon = new();
        public static UnityEvent OnTurnedoff = new();

        private void Start()
        {
            Reset();
        }

        public void Warm()
        {
            if (_warm <= 0) return;
            _warm -= 1;
            if (_warm <= 0) WarmExplosion();
            else 
            {
                if (_timerWarm == null) _timerWarm = new ResettableTimer(_during, TurnOff);
                _timerWarm.Start();
            }
        }

        public void WarmExplosion()
        {
            OnTurnedon.Invoke();
            _spriteRenderer.sprite = _enableSprite;
        }

        public void Reset()
        {
            _warm = _warmCapacity;
            _spriteRenderer.sprite = _disableSprite;
        }

        public void TurnOff()
        {
            Reset();
            Debug.Log("lever off " + transform.name);
            OnTurnedoff.Invoke();
        }
    }
}
