using Entities.PlayerScripts;
using Entities.UI;
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
        
        public override void InstallBindings()
        {
            Container.Bind<DialogueSystem>().FromNew().AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.Bind<DialogueVisuals>().FromInstance(_dialogueVisuals).AsSingle();
        }
    }
}