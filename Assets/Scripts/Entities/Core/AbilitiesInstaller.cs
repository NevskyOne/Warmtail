using Systems.AbilitiesVisual;
using UnityEngine;
using Zenject;

namespace Entities.Core
{
    public class AbilitiesInstaller : MonoInstaller
    {
        [SerializeField] private MetabolismVisualSt _metabolismVisualSt;
        public override void InstallBindings()
        {
            Container.Bind<MetabolismVisualSt>().FromInstance(_metabolismVisualSt).AsTransient();
        }
    }
    
}