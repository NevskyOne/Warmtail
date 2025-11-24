using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Systems;

namespace Entities.Core
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<PlacementSystem>().FromNew().AsSingle();
        }
    }
}
