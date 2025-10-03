using Systems;
using Systems.DataSystems;
using Systems.Dialogues;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entities.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GlobalDataSystem _globalData;
        [SerializeField] private DialogueSystem _dialogueSystem;
        [SerializeField] private PlayerInput _playerInput; 
        
        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().FromNew().AsSingle();
            Container.Bind<CrossfadeSystem>().FromNew().AsSingle();
            Container.Bind<GlobalDataSystem>().FromInstance(_globalData).AsSingle();
            Container.Bind<DialogueSystem>().FromInstance(_dialogueSystem).AsSingle();
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
        }
    }
}