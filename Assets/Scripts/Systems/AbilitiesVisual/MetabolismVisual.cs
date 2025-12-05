using System;
using System.Collections.Generic;
using Data.Player;
using Entities.PlayerScripts;
using Entities.Sound;
using Interfaces;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;
using Zenject;

namespace Systems.AbilitiesVisual
{
    public class MetabolismVisual : IAbilityVisual
    {
        [field: SerializeReference] public int AbilityIndex { get; set; }
        [SerializeField] private AudioClip _sfx;
        private ObjectSfx _playerSfx;
        private MetabolismVisualSt _struct;
        
        [Inject]
        private void Construct(PlayerConfig config, Player player, MetabolismVisualSt st)
        {
            var ability = config.Abilities[AbilityIndex];
            ability.StartAbility += StartAbility;
            ability.EndAbility += EndAbility;
            _playerSfx = player.ObjectSfx;
            _struct = st;
        }
        
        public void StartAbility()
        {
            _playerSfx.PlaySfx(_sfx);
            Tween.Custom(0, 1f, 1f, x => _struct.Volume.weight = x);
            _struct.HiddenWalls.ForEach(wall =>
            {
                var color = wall.color;
                color.a = 1;
                wall.color = color;
            });
        }

        public void UsingAbility()
        {
        }

        public void EndAbility()
        {
            Tween.Custom(1f, 0f, 1f, x => _struct.Volume.weight = x);
            _struct.HiddenWalls.ForEach(wall =>
            {
                var color = wall.color;
                color.a = 0;
                wall.color = color;
            });
        }
    }

    [Serializable]
    public struct MetabolismVisualSt
    {
        public List<SpriteShapeRenderer> HiddenWalls;
        public Volume Volume;
    }
}