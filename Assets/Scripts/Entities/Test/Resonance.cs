using System;
using Entities.PlayerScripts;
using Systems.Abilities;
using Systems.Swarm;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.Abilities.Concrete
{
    [Serializable]
    public class ResonanceAbility : BaseAbility
    {
        [SerializeField] private SwarmController _swarmPrefab;
        [SerializeField] private Vector3 _spawnOffset;
        
        private SwarmController _activeSwarm;
        private CinemachineCamera _vCam;
        private Transform _playerTransform;
        private WarmthSystem _warmthSystem;
        private Vector2 _input;

        [Inject]
        public void Construct(Player player, CinemachineCamera vCam, WarmthSystem warmth, PlayerInput input, DiContainer container)
        {
            _playerTransform = player.transform;
            _vCam = vCam;
            _warmthSystem = warmth;
            
            // Инстанциируем через Zenject, чтобы зависимости прокинулись в Boids
            _activeSwarm = container.InstantiatePrefabForComponent<SwarmController>(_swarmPrefab);
            _activeSwarm.Initialize();

            input.actions["Move"].performed += ctx => _input = ctx.ReadValue<Vector2>();
            
            StartAbility += OnStart;
            EndAbility += OnEnd;
            UsingAbility += OnTick;
        }

        private void OnStart()
        {
            _activeSwarm.Activate(_playerTransform.position + _spawnOffset);
            _vCam.Follow = _activeSwarm.transform;
        }

        private void OnTick()
        {
            if (!Enabled) return;

            bool isHeating = IsComboActive && _secondaryComboType == typeof(WarmingAbility);
            bool isAggressive = IsComboActive && _secondaryComboType == typeof(DashAbility);

            _activeSwarm.Move(_input);
            _activeSwarm.SetState(isAggressive, isHeating);
            
            _warmthSystem.DecreaseWarmth(1);
        }

        private void OnEnd()
        {
            _activeSwarm.Deactivate();
            _vCam.Follow = _playerTransform;
        }
    }
}
