using Data.Player;
using EditorOnly;
using Entities.PlayerScripts;
using Entities.UI;
using Entities.NPC;
using Interfaces;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Data;
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
        [SerializeField] private ShoppingManager _shoppingManager;
        [SerializeField] private SurfacingSystem _surfacingSystem;
        [SerializeField] private SwarmController _swarmController;
        
        public override void InstallBindings()
        {
            Container.Bind<SwarmController>().FromInstance(_swarmController).AsSingle();
            Container.Bind<SurfacingSystem>().FromInstance(_surfacingSystem).AsSingle();
            Container.Bind<DialogueSystem>().FromNew().AsSingle();
            Container.Bind<WarmthSystem>().FromNew().AsSingle();
            Container.Bind<ShoppingSystem>().FromNew().AsSingle();
            Container.Bind<NPCMethods>().FromNew().AsSingle();
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<DialogueVisuals>().FromInstance(_dialogueVisuals).AsSingle();
            Container.Bind<MonologueVisuals>().FromInstance(_monologueVisuals).AsSingle();
            Container.Bind<PopupSystem>().FromInstance(_popupSystem).AsSingle();
            Container.Bind<UIStateSystem>().FromInstance(_uiStateSystem).AsSingle();
            Container.Bind<CinemachineCamera>().FromInstance(_cam).AsSingle();
            Container.Bind<ShoppingManager>().FromInstance(_shoppingManager).AsSingle();
           
            Container.Inject(new KeysDebug());
            Container.Inject(new WarmthSystem());
            
            foreach (var ability in _playerConfig.Abilities)
            {
                Container.BindInterfacesAndSelfTo<IAbility>().FromInstance(ability).AsTransient();
            }
            Container.Bind<SceneLoader>().FromComponentInHierarchy().AsSingle();
        }

        
    }
}
