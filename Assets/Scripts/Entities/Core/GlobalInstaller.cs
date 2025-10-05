using Entities.PlayerScripts;
using Entities.UI;
using Systems;
using Systems.DataSystems;
using Systems.Effects;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entities.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GlobalDataSystem _globalData;
        [SerializeField] private DialogueVisuals _dialogueVisuals;
        [SerializeField] private Player _player; 
        [SerializeField] private PlayerInput _playerInput; 
        
        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().FromNew().AsSingle();
            Container.Bind<CrossfadeSystem>().FromNew().AsSingle();
            Container.Bind<DialogueSystem>().FromNew().AsSingle();
            Container.Bind<GlobalDataSystem>().FromInstance(_globalData).AsSingle();
            Container.Bind<DialogueVisuals>().FromInstance(_dialogueVisuals).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
        }
    }
}