using System;
using System.Collections.Generic;
using Data;
using Data.Player;
using Entities.Sound;
using Interfaces;
using Systems;
using Systems.Abilities.Concrete;
using Systems.DataSystems;
using UnityEngine;
using Zenject;

namespace Entities.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [field: SerializeReference] public Rigidbody2D Rigidbody { get; private set;}
        [field: SerializeReference] public ObjectSfx ObjectSfx { get; private set;}
        private GlobalData _globalData;
        private PlayerConfig _config;
        private DashAbility  _dashAbility;
        private PlayerMovement _movement;
        private List<IAbility> _disabledAbilities = new();
        private List<IDisposable> _disposables = new();
        
        [Inject]
        private void Construct(GlobalData globalData, PlayerConfig config, DiContainer container)
        {
            _globalData = globalData;
            _config = config;
            foreach (var ability in _config.Abilities)
            {
                container.Inject(ability);
                if(ability is IDisposable disposable)
                    _disposables.Add(disposable);
                
                if (ability.Visual != null)
                {
                    container.Inject(ability.Visual);
                    if (ability.Visual is IDisposable disposableVisual)
                        _disposables.Add(disposableVisual);
                }
            }

            _movement = (PlayerMovement)_config.Abilities[0];
            _dashAbility = (DashAbility)_config.Abilities[5];
        }

        private void FixedUpdate()
        {
            _movement.FixedTick();
            _dashAbility.Tick();
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

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
                disposable.Dispose();
            }
        }
    }
}
