using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Systems;

namespace Entities.Core
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private InputActionAsset _inputActions;

        public override void InstallBindings()
        {
            Container.Bind<InputActionAsset>().FromInstance(_inputActions).AsSingle();
            Container.Bind<PlacementSystem>().FromNew().AsSingle();
        }
    }
}
