using System;
using Cysharp.Threading.Tasks;
using Entities.PlayerScripts;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.WarmAbilities
{
    public class Warming : IAbility
    {
        [SerializeField] private int _warmDecrease;
        [SerializeField] private int _warmDelta;
        [SerializeField] private float _warmingRadius = 2f;
        [SerializeField] private Vector3 _warmingOffset = Vector3.zero;
        
        private Player _player;
        private WarmthSystem _warmthSystem;
        private bool _isWarming;
    
        public bool Enabled { get; set; }
        public Action StartAbility { get; set; }
        public Action UsingAbility { get; set; }
        public Action EndAbility { get; set; }
        
        [Inject]
        public void Construct(Player player, PlayerInput playerInput, WarmthSystem warmthSystem)
        {
            _player = player;
            _warmthSystem = warmthSystem;
            playerInput.actions["RightMouse"].started += _ =>
            {
                StartAbility?.Invoke();
                _isWarming = true;
                Use();
            };
            playerInput.actions["RightMouse"].canceled += _ =>
            {
                EndAbility?.Invoke();
                _isWarming = false;
            };
        }
        
        public async void Use()
        {
            while (Enabled)
            {
                await UniTask.Delay(_warmDelta);
                if (!_isWarming) return;
                var colliders = Physics2D.OverlapCircleAll(
                    _player.Rigidbody.transform.position + _warmingOffset, _warmingRadius);

                bool found = false;
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<IWarmable>(out var warmable))
                    {
                        warmable.Warm();
                        found = true;
                    }
                }

                if (found)
                {
                    _warmthSystem.DecreaseWarmth(_warmDecrease);
                    UsingAbility?.Invoke();
                }

            }
        }
    }
}