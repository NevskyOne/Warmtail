using UnityEngine;
using System;
using Zenject;

namespace Entities.Core
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private PlacementSystem _placementSystem;

        public override void InstallBindings()
        {
            Container.Bind<PlacementSystem>().FromInstance(_placementSystem).AsSingle();
            Container.Bind<HouseData>().FromNew().AsSingle();
        }
    }
}
