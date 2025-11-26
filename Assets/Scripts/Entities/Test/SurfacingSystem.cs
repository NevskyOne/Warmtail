using System.Collections.Generic;
using Data;
using Data.Player;
using UnityEngine;
using Zenject;

namespace Systems.Environment
{
    public class SurfacingSystem : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _levelRoots;
        [SerializeField] private float _layerCheckRadius = 1f;
        [SerializeField] private LayerMask _surfaceTriggerLayer;
        [SerializeField] private LayerMask _obstacleLayer;

        private GlobalData _globalData;
        private int _currentLayerIndex = 0;

        [Inject]
        public void Construct(GlobalData globalData)
        {
            Debug.Log("вызов SurfacingSystem");
            _globalData = globalData;
            UpdateLevelVisibility();
        }

        public bool TryChangeLayer(int direction)
        {
            Debug.Log($"вызов , direction = {direction}");

            var newIndex = _currentLayerIndex + direction;
            var maxLayers = _globalData.Get<SavablePlayerData>().ActiveLayers;

            Debug.Log($"текущий слой: {_currentLayerIndex}, новый слой: {newIndex}, максимальный: {maxLayers}");

            if (newIndex < 0 || newIndex > maxLayers || newIndex >= _levelRoots.Count)
            {
                Debug.Log("TryChangeLayer неудачно: вне диапазона");
                return false;
            }

            if (!CanTransition(direction))
            {
                Debug.Log("TryChangeLayer неудачно: CanTransition вернул false");
                return false;
            }

            _currentLayerIndex = newIndex;
            UpdateLevelVisibility();

            Debug.Log($"Слой изменен на {_currentLayerIndex}");
            return true;
        }


        private bool CanTransition(int direction)
        {
            // Если всплываем (E), проверяем, нет ли льда сверху и находимся ли в зоне
            if (direction > 0)
            {
                var pos = transform.position;
                bool inZone = Physics2D.OverlapCircle(pos, _layerCheckRadius, _surfaceTriggerLayer);
                bool blocked = Physics2D.Raycast(pos, Vector2.up, 3f, _obstacleLayer);
                return inZone && !blocked;
            }
            return true; // Вниз обычно можно всегда (по геймдизайну)
        }

        private void UpdateLevelVisibility()
        {
            for (int i = 0; i < _levelRoots.Count; i++)
            {
                if (_levelRoots[i] != null)
                    _levelRoots[i].SetActive(i == _currentLayerIndex);
            }
        }
    }
}
