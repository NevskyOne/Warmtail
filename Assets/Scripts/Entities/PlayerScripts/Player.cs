using Data;
using Data.Player;
using Systems.DataSystems;
using UnityEngine;
using Zenject;

namespace Entities.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [field: SerializeReference] public Rigidbody2D Rigidbody { get; private set;}
        private GlobalData _globalData;
        private PlayerConfig _config;
        
        [Inject]
        private void Construct(GlobalData globalData, PlayerConfig config)
        {
            _globalData = globalData;
            _config = config;
            _config.Abilities[0].Enabled = true;
        }
    }
}