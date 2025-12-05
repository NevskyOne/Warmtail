using UnityEngine;

namespace Entities.Puzzle
{
    public class RayCollector : MonoBehaviour
    {
        [SerializeField] private RayCollector _parentRayCollector;
        [SerializeField] private RayPuzzle _rayPuzzle;
        [SerializeField] private GameObject _rayImage;

        private int _incomingRayCount;
        private int _correctSignals;

        private void Start()
        {
            if (_parentRayCollector) _parentRayCollector.AddIncoming();
            if (_rayImage) _rayImage.SetActive(false);
        }

        public void AddIncoming()
        {
            _incomingRayCount ++;
        }

        public void AddCorrectSignal()
        {
            _correctSignals ++;
            if (_correctSignals == _incomingRayCount) Complete();
        }

        public void DicreaseCorrectSignal()
        {
            _correctSignals --;
            if (_correctSignals == _incomingRayCount-1) Cancel();
        }

        private void Complete()
        {
            if (_parentRayCollector) 
            {
                _rayImage.SetActive(true);
                _parentRayCollector.AddCorrectSignal();
            }
            else _rayPuzzle.Solve();
        }

        private void Cancel()
        {
            if (_parentRayCollector) 
            {
                _rayImage.SetActive(false);
                _parentRayCollector.DicreaseCorrectSignal();
            }
        }
    }
}
