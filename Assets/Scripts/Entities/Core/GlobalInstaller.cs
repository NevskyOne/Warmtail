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
            Container.Bind<CrossfadeEffect>().FromNew().AsSingle();
            #if UNITY_EDITOR
            Container.Inject(_globalData);
            #endif
            Container.Bind<GlobalData>().FromInstance(_globalData).AsSingle();
        }
    }
}