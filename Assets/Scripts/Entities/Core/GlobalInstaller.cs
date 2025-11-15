using System;
using Data;
using Data.Player;
using Interfaces;
using Systems.DataSystems;
using Systems.Effects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Entities.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GlobalData _globalDataPrefab;
        private GlobalData _globalData;

        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().FromNew().AsSingle();
            Container.Bind<ManualSaveSystem>().FromNew().AsSingle();
            Container.Bind<CrossfadeEffect>().FromNew().AsSingle();
            _globalData = Instantiate(_globalDataPrefab, transform);
            Container.Inject(_globalData);
            Container.Bind<GlobalData>().FromInstance(_globalData).AsSingle();
           
        }
    }
}