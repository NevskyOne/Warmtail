using System;
using Cysharp.Threading.Tasks;
using Entities.PlayerScripts;
using Systems.Swarm;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System.Reflection;
using System.Threading;

namespace Systems.Abilities.Concrete
{
    [Serializable]
    public class ResonanceAbility : BaseAbility
    {
        [SerializeField] private float _searchRadius = 12f;
        [SerializeField] private float _interactRadius = 5f;
        [SerializeField] private float _warmthDrainPerSecond = 2f;
        [SerializeField] private float _tickDelay = 0.1f;

        private WarmthSystem _warmthSystem;
        private Transform _playerTransform;
        private Player _player;
        private CinemachineCamera _vCam;
        private PlayerInput _input;
        private SwarmController _activeSwarm;
        private Vector2 _moveInput;

        private float _warmthAccumulator;
        private CancellationTokenSource _tickCts;

        [Inject]
        public void Construct(Player player, WarmthSystem warmth, PlayerInput input, CinemachineCamera cam)
        {
            _player = player;
            _playerTransform = player.Rigidbody.transform;
            _warmthSystem = warmth;
            _input = input;
            _vCam = cam;
            
            var moveAction = _input.actions.FindAction("Move");
            if (moveAction != null)
            {
                moveAction.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
                moveAction.canceled += _ => _moveInput = Vector2.zero;
            }

            StartAbility += OnStart;
            EndAbility += OnEnd;
        }

        private void OnStart()
        {
            _activeSwarm = FindNearestSwarm();
            if (_activeSwarm == null)
            {
                return;
            }
            
            if (_player != null)
                _player.StartResonance(_activeSwarm.GetComponent<Rigidbody2D>());

            _activeSwarm.SetControlled(true);
            
            if (_vCam != null)
            {
                _vCam.Follow = _activeSwarm.transform;
                _vCam.LookAt = _activeSwarm.transform;
            }

            _warmthAccumulator = 0f;

            _tickCts?.Cancel();
            _tickCts = new CancellationTokenSource();
            TickCycle(_tickCts.Token).Forget();

        }

        private async UniTaskVoid TickCycle(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && Enabled)
                {
                    if (_activeSwarm == null)
                        break;

                    ProcessSwarmInteraction();
                    await UniTask.Delay(TimeSpan.FromSeconds(_tickDelay), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void ProcessSwarmInteraction()
        {
            bool anyNear = false;
            var neighbors = _activeSwarm.GetNeighbors(null);
            Vector2 playerPos = _playerTransform.position;

            foreach (var boid in neighbors)
            {
                float dist = Vector2.Distance(playerPos, boid.transform.position);
                if (dist <= _interactRadius)
                {
                    anyNear = true;
                    boid.InteractWithPhysicsObjects();
                }
            }

            if (!anyNear)
            {
                _activeSwarm.SetControlInput(Vector2.zero);
                return;
            }
            _activeSwarm.SetControlInput(_moveInput);
            
            ApplyWarmthDrain();
        }

        private void ApplyWarmthDrain()
        {
            float drain = _warmthDrainPerSecond * _tickDelay;
            _warmthAccumulator += drain;

            int consumeUnits = Mathf.FloorToInt(_warmthAccumulator);
            if (consumeUnits <= 0)
                return;

            if (!_warmthSystem.CheckWarmCost(consumeUnits))
            {
                EndAbility?.Invoke();
                return;
            }

            _warmthSystem.DecreaseWarmth(consumeUnits);
            _warmthAccumulator -= consumeUnits;
        }

        private void OnEnd()
        {
            _tickCts?.Cancel();

            if (_activeSwarm != null)
                _activeSwarm.SetControlled(false);

            if (_player != null)
                _player.StopResonance();

            if (_vCam != null && _playerTransform != null)
            {
                _vCam.Follow = _playerTransform;
                _vCam.LookAt = _playerTransform;
            }

            _moveInput = Vector2.zero;
            _warmthAccumulator = 0f;
            _activeSwarm = null;
            
        }

        private SwarmController FindNearestSwarm()
        {
            var swarms = GameObject.FindObjectsOfType<SwarmController>();
            SwarmController nearest = null;

            float bestDist = float.MaxValue;
            Vector2 p = _playerTransform.position;

            foreach (var s in swarms)
            {
                float d = Vector2.Distance(p, s.transform.position);
                if (d < bestDist && d <= _searchRadius)
                {
                    bestDist = d;
                    nearest = s;
                }
            }
            return nearest;
        }
    }
}
