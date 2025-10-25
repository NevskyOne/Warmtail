using Data;
using Entities.Sound;
using UnityEngine;
using Zenject;

namespace Entities.Core
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private MusicStateSystem _musicSystem;

        [Inject]
        private void Construct(GlobalData globalData)
        {
            Container.Inject(globalData);
        }
        
        public override void InstallBindings()
        {
            Container.Bind<MusicStateSystem>().FromInstance(_musicSystem).AsSingle();
        }
    }
}