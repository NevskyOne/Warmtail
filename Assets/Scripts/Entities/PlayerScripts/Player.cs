using System;
using Data;
using Data.Player;
using Systems;
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
        private PlayerMovement _movement;
        
        [Inject]
        private void Construct(GlobalData globalData, PlayerConfig config)
        {
            _globalData = globalData;
            _config = config;
            foreach (var ability in _config.Abilities)
            {
                ability.Enabled = true;
            }

            _movement = (PlayerMovement)_config.Abilities[0];
        }

        private void FixedUpdate()
        {
            _movement.FixedTick();
        }
    }
}
