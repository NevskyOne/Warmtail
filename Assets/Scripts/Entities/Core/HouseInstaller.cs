using Data.Player;
using Entities.PlayerScripts;
using Entities.UI;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Systems;
using Entities.House;

namespace Entities.Core
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private HouseManager _houseManager;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private UIStateSystem _uiStateSystem;

        public override void InstallBindings()
        {
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<HouseManager>().FromInstance(_houseManager).AsSingle();
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.Bind<UIStateSystem>().FromInstance(_uiStateSystem).AsSingle();
            
            Container.Bind<PlacementSystem>().FromNew().AsSingle();
            Container.Bind<DialogueSystem>().FromNew().AsSingle();
            
            foreach (var ability in _playerConfig.Abilities)
            {
                Container.BindInterfacesAndSelfTo<IAbility>().FromInstance(ability).AsTransient();
            }
        }
    }
}
