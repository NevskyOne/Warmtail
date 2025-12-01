using System.Collections.Generic;
using System.Linq;
using Data.Player;
using EditorOnly;
using Entities.PlayerScripts;
using Entities.UI;
using Interfaces;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Data;
using Systems.Abilities;
using Systems.Abilities.Concrete;
using Systems.Environment;
using Systems.Swarm;
using Unity.Cinemachine;
using UnityEditor;

namespace Entities.Core
{
    public class NormalSceneInstaller : MonoInstaller
    {
        [SerializeField] private DialogueVisuals _dialogueVisuals;
        [SerializeField] private MonologueVisuals _monologueVisuals;
        [SerializeField] private Player _player; 
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private PopupSystem _popupSystem;
        [SerializeField] private UIStateSystem _uiStateSystem;
        [SerializeField] private CinemachineCamera _cam;
        [SerializeField] private SurfacingSystem _surfacingSystem;
        [SerializeField] private SwarmController _swarmController;
        
       
        public override void InstallBindings()
        {
            Container.Bind<SwarmController>().FromInstance(_swarmController).AsSingle();
            Container.Bind<SurfacingSystem>().FromInstance(_surfacingSystem).AsSingle();
            Container.Bind<DialogueSystem>().FromNew().AsSingle();
            Container.Bind<WarmthSystem>().FromNew().AsSingle();
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<DialogueVisuals>().FromInstance(_dialogueVisuals).AsSingle();
            Container.Bind<MonologueVisuals>().FromInstance(_monologueVisuals).AsSingle();
            Container.Bind<PopupSystem>().FromInstance(_popupSystem).AsSingle();
            Container.Bind<UIStateSystem>().FromInstance(_uiStateSystem).AsSingle();
            Container.Bind<CinemachineCamera>().FromInstance(_cam).AsSingle();
            
            Container.Inject(new KeysDebug()); 
            Container.Inject(new WarmthSystem());
            
            Container.Bind<List<IAbility>>()
                .FromInstance(_playerConfig.Abilities)
                .AsSingle();
            
            var extendedAbilities = _playerConfig.Abilities
                .OfType<IAbilityExtended>()
                .ToList();
            
            Container.Bind<List<IAbilityExtended>>()
                .WithId("PlayerAbilities")
                .FromInstance(extendedAbilities)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<AbilitiesManager>().AsSingle();
            
            Container.Bind<SceneLoader>().FromComponentInHierarchy().AsSingle();
        }
    }
}
