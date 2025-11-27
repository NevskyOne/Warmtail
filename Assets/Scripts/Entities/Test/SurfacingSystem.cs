using System.Collections.Generic;
using Data;
using Data.Player;
using Entities.PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;
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
        private Player _player;
        private int _currentLayerIndex;

        [Inject]
        public void Construct(GlobalData globalData, PlayerInput input, Player player)
        {
            _globalData = globalData;
            _player = player;
            UpdateLevelVisibility();
            input.actions["Surfacing"].started += ctx =>
            {
                var direction = (int)ctx.ReadValue<float>();
                TryChangeLayer(direction);
            };
        }

        public bool TryChangeLayer(int direction)
        {
            var newIndex = _currentLayerIndex + (int)direction;
            var maxLayers = _globalData.Get<SavablePlayerData>().ActiveLayers;

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
            return true;
        }


        private bool CanTransition(float direction)
        {
            if (direction > 0)
            {
                var pos = _player.Rigidbody.transform.position;
                bool inZone = Physics2D.OverlapCircle(pos, _layerCheckRadius, _surfaceTriggerLayer);
                bool blocked = Physics2D.Raycast(pos, Vector2.up, 3f, _obstacleLayer);
                return inZone && !blocked;
            }
            return true;
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
