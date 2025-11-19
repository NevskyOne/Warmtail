using Data;
using Entities.Localization;
using Entities.Sound;
using UnityEngine;
using Zenject;

namespace Entities.Core
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private MusicStateSystem _musicSystem;
        [SerializeField] private SceneLoader _sceneLoader;
        
        public override void InstallBindings()
        {
            Container.Bind<MusicStateSystem>().FromInstance(_musicSystem).AsSingle();
            Container.Bind<SceneLoader>().FromInstance(_sceneLoader).AsSingle();
        }
    }
}