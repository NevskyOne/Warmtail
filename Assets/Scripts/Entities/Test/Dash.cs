using System;
using Data.Player;
using Entities.PlayerScripts;
using Interfaces;
using Systems.Abilities;
using Systems.Environment;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Cysharp.Threading.Tasks;


namespace Systems.Abilities.Concrete
{
    [Serializable]
    public class DashAbility : BaseAbility, ITickable
    {
        [SerializeField] private int _dashCost = 15;
        [SerializeField] private float _destroyRadius = 1.5f;
        [SerializeField] private float dashCooldownDuration = 1f;
        private float lastDashTime = -Mathf.Infinity;
        private bool _dashLoopRunning = false;


        private PlayerConfig _playerConfig;
        private Rigidbody2D _playerRb;
        private SurfacingSystem _surfacingSystem;
        private WarmthSystem _warmthSystem;

        private Vector2 _moveInput;
        private float _layerInput;

        [Inject]
        public void Construct(PlayerConfig playerConfig, Player player, WarmthSystem warmth, SurfacingSystem surfacing,
            PlayerInput input, DiContainer container)
        {

            _playerRb = player.Rigidbody;
            _surfacingSystem = surfacing;
            _warmthSystem = warmth;
            _playerConfig = playerConfig;

            input.actions["Move"].performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            input.actions["Move"].canceled += _ => _moveInput = Vector2.zero;
            
            input.actions["Surfacing"].performed += ctx => _layerInput = ctx.ReadValue<float>();
            input.actions["Surfacing"].canceled += _ => _layerInput = 0;

            UsingAbility += Tick;
        }


        public async void Tick()
        {
           
            Debug.Log("Tick");
            if (!Enabled) return;
            bool isFree = IsComboActive && _secondaryComboType == typeof(MetabolismAbility);

            if (Mathf.Abs(_layerInput) > 0.1f)
            {
                Debug.Log("ChangeLayer");
                int dir = (int)Mathf.Sign(_layerInput);
                if (_surfacingSystem.TryChangeLayer(dir))
                {
                    if (!isFree) _warmthSystem.DecreaseWarmth(_dashCost);
                    _layerInput = 0;
                }
            }

            if (_moveInput.magnitude > 0.1f )
            {
                if (!_dashLoopRunning)
                {
                    _dashLoopRunning = true;
                    DashLoop();
                }
            }
            else
            {
                _dashLoopRunning = false;
            }


             async void DashLoop()
            {
                while (_dashLoopRunning && _moveInput.magnitude > 0.1f )
                {
                    Dash();
                    
                    await UniTask.Delay(500);
                }
                
                _dashLoopRunning = false;
                ((PlayerMovement)_playerConfig.Abilities[0]).MoveForce = 60;
            }


             void Dash()
            {
                HandleObstacleDestruction();
                ((PlayerMovement)_playerConfig.Abilities[0]).MoveForce = 100;

                bool isFree = IsComboActive && _secondaryComboType == typeof(MetabolismAbility);

                if (!isFree)
                    _warmthSystem.DecreaseWarmth(_dashCost);

                Debug.Log("Dash");
            }

            void HandleObstacleDestruction()
            {
                var hits = Physics2D.OverlapCircleAll(_playerRb.position, _destroyRadius);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<IDestroyable>(out var dest))
                        dest.DestroyObject();
                }
            }
            await UniTask.Delay(500);
        }
    }
    
}
