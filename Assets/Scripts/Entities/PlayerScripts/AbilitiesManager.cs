using System;
using System.Collections.Generic;
using Data;
using Data.Player;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.Abilities
{
    [Serializable]
    public class AbilitiesManager : ITickable
    {
        private  List<IAbilityExtended> _abilities;
        private  GlobalData _globalData;
        
        [SerializeField] private int _selectedIndex;
        [SerializeField] private IAbilityExtended _activeAbility;
        [SerializeField] private IAbilityExtended _comboAbility;
        [SerializeField] private bool _isCasting;
        public IAbilityExtended ActiveAbility => _activeAbility;
        public IAbilityExtended ComboAbility => _comboAbility;
        
        [Inject]
        public void Construct( 
            [Inject(Id = "PlayerAbilities")] List<IAbilityExtended> abilities, 
            WarmthSystem warmthSystem,
            GlobalData globalData,
            PlayerInput input)
        {
            _abilities = abilities;
            _globalData = globalData;
            SetupInput(input);
            SelectAbility(0);
        }

        private void SetupInput(PlayerInput input)
        {
            input.actions["Scroll"].performed += ctx => CycleSelection(ctx.ReadValue<Vector2>().y);

            input.actions["1"].performed += _ => TrySelectOrCombo(0);
            input.actions["2"].performed += _ => TrySelectOrCombo(1);
            input.actions["3"].performed += _ => TrySelectOrCombo(2);
            input.actions["4"].performed += _ => TrySelectOrCombo(3);

            input.actions["RightMouse"].started += _ => StartCasting();
            input.actions["RightMouse"].canceled += _ => StopCasting();
        }

        private void CycleSelection(float scrollValue)
        {
            if (_isCasting) return;
            
            if (scrollValue > 0) SelectAbility(_selectedIndex + 1);
            else if (scrollValue < 0) SelectAbility(_selectedIndex - 1);
        }

        private void SelectAbility(int index)
        {
            Debug.Log("Select Ability: " + index);
            if (index < 0) index = _abilities.Count - 1;
            if (index >= _abilities.Count) index = 0;

            _selectedIndex = index;
        }

        private void TrySelectOrCombo(int index)
        {
            Debug.Log("Try selecting " + index);
            if (index < 0 || index >= _abilities.Count) return;

            if (_isCasting)
            {
                var candidate = _abilities[index];
                if (candidate != _activeAbility)
                {
                    ActivateCombo(candidate);
                }
            }
            else
            {
                SelectAbility(index);
            }
        }

        private void StartCasting()
        {
            
            if (_activeAbility != null) return;
            
            if (_globalData.Get<RuntimePlayerData>().CurrentWarmth <= 0) return;
            _activeAbility = _abilities[_selectedIndex];
            _activeAbility.Enabled = true;
            _activeAbility.ResetCombo();
            _activeAbility.StartAbility?.Invoke();
            _isCasting = true;
        }

        private void ActivateCombo(IAbilityExtended secondary)
        {
            if (_comboAbility != null) return;

            _comboAbility = secondary;
            _activeAbility.SetComboContext(_comboAbility.AbilityType);
        }

        private void StopCasting()
        {
            if (_activeAbility == null) return;
            _activeAbility.ResetCombo();
            _activeAbility.Enabled = false;
            _activeAbility.EndAbility?.Invoke();
            _activeAbility = null;
            _comboAbility = null;
            _isCasting = false;
        }

        public void Tick()
        {
            if (_isCasting && _globalData.Get<RuntimePlayerData>().CurrentWarmth <= 0)
            {
                StopCasting();
            }
        }
    }
}
