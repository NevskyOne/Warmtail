using System;
using Data;
using Entities.Localization;
using Systems.DataSystems;
using Systems.Effects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Entities.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private GlobalData _globalData;
        [SerializeField] private LocalizationManager _localizationManager;

        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().FromNew().AsSingle();
            Container.Bind<CrossfadeEffect>().FromNew().AsSingle();
            Container.Bind<LocalizationManager>().FromInstance(_localizationManager).AsSingle();
            Container.Bind<GlobalData>().FromInstance(_globalData).AsSingle();
        }
    }
}