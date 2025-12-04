using UnityEngine;
using Interfaces;
using Systems;

namespace Entities.Puzzle
{
    public class RayTrigger : MonoBehaviour, IWarmable
    {
        [SerializeField] private RayCollector _parentRayCollector;
        [SerializeField] private bool _isActiveInitially;
        [SerializeField] private int _warmCapacity;
        [SerializeField] private GameObject _rayImage;

        private int _warm;
        private bool _isActive;
        private ResettableTimer _timerWarm;

        private void Start()
        {
            if (!_isActiveInitially) _rayImage.SetActive(false);
            _warm = _warmCapacity;
            _isActive = _isActiveInitially;
            _parentRayCollector.AddIncoming();
        }

        public void Warm()
        {
            if (_warm <= 0) return;
            _warm -= 1;
            if (_warm <= 0) WarmExplosion();
            else 
            {
                if (_timerWarm == null) _timerWarm = new ResettableTimer(3, Reset);
                _timerWarm.Start();
            }
        }

        public void WarmExplosion()
        {
            _isActive = !_isActive;
            _rayImage.SetActive(_isActive);
            if (_isActive != _isActiveInitially) _parentRayCollector.AddCorrectSignal();
            else _parentRayCollector.DicreaseCorrectSignal();
        }

        public void Reset()
        {
            _warm = _warmCapacity;
        }
    }
}
