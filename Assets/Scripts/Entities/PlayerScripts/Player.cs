using System;
using System.Collections.Generic;
using Data;
using Data.Player;
using Interfaces;
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
        private List<IAbility> _disabledAbilities = new();
        
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

        public void DisableAllAbilities()
        {
            foreach (var ability in _config.Abilities)
            {
                if (ability.Enabled)
                {
                    _disabledAbilities.Add(ability);
                    ability.Enabled = false;
                }
            }
        }
        
        public void EnableLastAbilities()
        {
            foreach (var ability in _disabledAbilities)
            {
                ability.Enabled = true;
            }
            _disabledAbilities.Clear();
        }
    }
}
