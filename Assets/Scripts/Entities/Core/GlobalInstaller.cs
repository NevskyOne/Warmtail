using Systems.DataSystems;
using UnityEngine;
using Zenject;

namespace Entities.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GlobalDataSystem _globalData;
        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().FromNew().AsSingle();
            Container.Bind<GlobalDataSystem>().FromInstance(_globalData).AsSingle();
        }
    }
}