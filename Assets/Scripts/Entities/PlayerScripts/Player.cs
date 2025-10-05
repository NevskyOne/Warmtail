using Systems.DataSystems;
using UnityEngine;
using Zenject;

namespace Entities.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        private GlobalDataSystem _globalDataSystem;
        
        [Inject]
        private void Construct(GlobalDataSystem globalDataSystem)
        {
            _globalDataSystem = globalDataSystem;
        }
    }
}