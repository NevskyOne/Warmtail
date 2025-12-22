using UnityEngine;
using UnityEngine.Events;

namespace Interfaces
{
    public abstract class Warmable : MonoBehaviour
    {
        [SerializeField] protected float _warmFactor = 0.1f;
        protected float _warmthAmount = 1;
        protected float _maxWarmthAmount = 1;
        [SerializeField] protected UnityEvent _warmEvent;

        public virtual void Warm()
        {
            if (_warmthAmount <= 0) return;
            _warmthAmount -= _warmFactor;
            if (_warmthAmount <= 0)
                WarmComplete();
        }

        public virtual void WarmComplete()
        {
            _warmEvent.Invoke();
        }

        public virtual void Reset()
        {
            _warmthAmount = _maxWarmthAmount;
        }
    }
}