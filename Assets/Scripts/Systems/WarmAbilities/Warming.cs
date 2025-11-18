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
        public bool Enabled { get; set; }
        private Player _player;
        private WarmthSystem _warmthSystem;
        private bool _isWarming;
    
        [Inject]
        public void Construct(Player player, PlayerInput playerInput, WarmthSystem warmthSystem)
        {
            _player = player;
            _warmthSystem = warmthSystem;
            playerInput.actions["RightMouse"].started += _ =>
            {
                _isWarming = true;
                Use();
            };
            playerInput.actions["RightMouse"].canceled += _ =>
            {
                _isWarming = false;
            };
        }
        
        public async void Use()
        {
            while (Enabled && _isWarming)
            {
                var colliders = Physics2D.OverlapCircleAll(
                    _player.transform.position + _warmingOffset, _warmingRadius);

                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<IWarmable>(out var warmable))
                    {
                        warmable.Warm();
                    }
                }
                _warmthSystem.DecreaseWarmth(_warmDecrease);
                await UniTask.Delay(_warmDelta);
            }
        }
    }
}