using Data;
using Systems.DataSystems;
using Systems.Effects;
using UnityEngine;
using Zenject;

namespace Entities.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GlobalData _globalData;
    
        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().FromNew().AsSingle();
            Container.Bind<CrossfadeSystem>().FromNew().AsSingle();
            Container.Inject(_globalData);
            Container.Bind<GlobalData>().FromInstance(_globalData).AsSingle();
        }
    }
}