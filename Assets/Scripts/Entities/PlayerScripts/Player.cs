using Data;
using Systems.DataSystems;
using UnityEngine;
using Zenject;

namespace Entities.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        private GlobalData _globalData;
        
        [Inject]
        private void Construct(GlobalData globalData)
        {
            _globalData = globalData;
        }
    }
}