using UnityEngine.Events;
using UnityEngine;
using Interfaces;
using Systems;

namespace Entities.Puzzle
{
    public class Gear : Warmable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _twistingSprite;
        [SerializeField] private Sprite _untwistSprite;
        
        private int _gearId;
        private ResettableTimer _timerWarm;

        public static UnityEvent<int> OnTwisted = new();

        public void Initialize(int id)
        {
            _gearId = id;
            Reset();
            GearsPuzzle.OnReseted.AddListener(Reset);
        }
        
        public override void Warm()
        {
            base.Warm();
            if (_warmthAmount > 0 && _timerWarm != null) _timerWarm.Start();
            else if(_warmthAmount > 0) _timerWarm = new ResettableTimer(_maxWarmthAmount, WarmLost);
        }

        public override void WarmComplete() // twisting
        {
            _spriteRenderer.sprite = _twistingSprite;
            OnTwisted.Invoke(_gearId);
        }

        private void WarmLost() // untwist
        {
            if (_warmthAmount > 0) Reset();
        }

        public override void Reset() // untwist
        {
            base.Reset();
            _spriteRenderer.sprite = _untwistSprite;
        }
    }
}
