using UnityEngine;
using Zenject;
using Systems.DataSystems;

namespace Entities.Player
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