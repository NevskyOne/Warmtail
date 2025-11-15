using Data.Player;
using EditorOnly;
using Entities.PlayerScripts;
using Entities.UI;
using Interfaces;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entities.Core
{
    public class NormalSceneInstaller : MonoInstaller
    {
        [SerializeField] private DialogueVisuals _dialogueVisuals;
        [SerializeField] private Player _player; 
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private PopupSystem _popupSystem;
        [SerializeField] private UIStateSystem _uiStateSystem;
        
        public override void InstallBindings()
        {
            Container.Bind<DialogueSystem>().FromNew().AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<DialogueVisuals>().FromInstance(_dialogueVisuals).AsSingle();
            Container.Bind<PopupSystem>().FromInstance(_popupSystem).AsSingle();
            Container.Bind<UIStateSystem>().FromInstance(_uiStateSystem).AsSingle();
            Container.Inject(new KeysDebug());
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            foreach (var ability in _playerConfig.Abilities)
            {
                Container.Inject(ability);
                Container.BindInterfacesAndSelfTo<IAbility>().FromInstance(ability).AsSingle();
            }
        }
    }
}