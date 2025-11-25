using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Systems;
using Entities.House;

namespace Entities.Core
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private HouseManager _houseManager;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<HouseManager>().FromInstance(_houseManager).AsSingle();
            Container.Bind<PlacementSystem>().FromNew().AsSingle();
        }
    }
}
