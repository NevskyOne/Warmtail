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
            _globalData = globalData;
            UpdateLevelVisibility();
        }

        public bool TryChangeLayer(int direction) // -1 Down (Q), +1 Up (E)
        {
            var newIndex = _currentLayerIndex + direction;
            var maxLayers = _globalData.Get<SavablePlayerData>().ActiveLayers;

            if (newIndex < 0 || newIndex > maxLayers || newIndex >= _levelRoots.Count)
                return false;

            // Проверка физической возможности (триггеры/препятствия)
            if (!CanTransition(direction)) return false;

            _currentLayerIndex = newIndex;
            UpdateLevelVisibility();
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