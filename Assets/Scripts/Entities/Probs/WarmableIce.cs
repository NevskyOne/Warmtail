using Interfaces;
using Systems;
using UnityEngine;

namespace Entities.Probs
{
    public class WarmableIce : MonoBehaviour, IWarmable
    {
        [SerializeField] private int _warmCapacity;
        private int _warm;
        private ResettableTimer _timer;

        private void Start()
        {
            Reset();
        }
        
        public void Warm()
        {
            _warm -= 1;
            if (_warm <= 0)
            {
                WarmExplosion();
            }
            else
            {
                if (_timer != null)
                    _timer.Start();
                else
                    _timer = new ResettableTimer(3, Reset);
            }
        }

        public void WarmExplosion()
        {
            _timer.Stop();
            Destroy(gameObject);
        }

        public void Reset()
        {
            _warm = _warmCapacity;
        }
    }
}