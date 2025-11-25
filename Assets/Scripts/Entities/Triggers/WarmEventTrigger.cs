using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Triggers
{
    public class WarmEventTrigger : MonoBehaviour, IWarmable
    {
        [SerializeField] private int _warmCapacity;
        [SerializeField] private UnityEvent _warmAction = new();
        private int _warm;

        private void Start() => Reset();
        
        public void Warm()
        {
            _warm -= 1;
            if (_warm <= 0)
                WarmExplosion();
        }

        public void WarmExplosion()
        {
            _warmAction.Invoke();
        }

        public void Reset()
        {
            _warm = _warmCapacity;
        }
    }
}