using UnityEngine.Events;
using UnityEngine;
using Interfaces;
using Systems;

namespace Entities.Puzzle
{
    public class Lever : Warmable
    {
        [SerializeField] private float _during;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _enableSprite;
        [SerializeField] private Sprite _disableSprite;
        
        private ResettableTimer _timerWarm;

        public static UnityEvent OnTurnedon = new();
        public static UnityEvent OnTurnedoff = new();

        private void Start()
        {
            Reset();
        }

        public override void Warm()
        {
            base.Warm();
            if(_warmthAmount > 0)
            {
                _timerWarm ??= new ResettableTimer(_during, TurnOff);
                _timerWarm.Start();
            }
        }

        public override void WarmComplete()
        {
            OnTurnedon.Invoke();
            _spriteRenderer.sprite = _enableSprite;
        }

        public override void Reset()
        {
            base.Reset();
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
