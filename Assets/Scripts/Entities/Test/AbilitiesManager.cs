using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Player;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.Abilities
{
    public class AbilitiesManager : ITickable
    {
        public event Action<int> OnAbilitySelected;
        public event Action<Type, Type> OnComboUpdated; // Primary, Secondary

        private readonly List<IAbilityExtended> _abilities;
        private readonly WarmthSystem _warmthSystem;
        private readonly GlobalData _globalData;
        
        private int _selectedIndex;
        private IAbilityExtended _activeAbility;
        private IAbilityExtended _comboAbility; // Вторичная способность (пассивная часть комбо)
        private bool _isCasting;

        [Inject]
        public AbilitiesManager(
            [Inject(Id = "PlayerAbilities")] List<IAbilityExtended> abilities, 
            WarmthSystem warmthSystem,
            GlobalData globalData,
            PlayerInput input)
        {
            _abilities = abilities;
            _warmthSystem = warmthSystem;
            _globalData = globalData;
            SetupInput(input);
            SelectAbility(0);
        }

        private void SetupInput(PlayerInput input)
        {
            // Скролл
            input.actions["Scroll"].performed += ctx => CycleSelection(ctx.ReadValue<float>());
            
            // Выбор цифрами (1-4)
            input.actions["SelectAbility1"].performed += _ => TrySelectOrCombo(0);
            input.actions["SelectAbility2"].performed += _ => TrySelectOrCombo(1);
            input.actions["SelectAbility3"].performed += _ => TrySelectOrCombo(2);
            input.actions["SelectAbility4"].performed += _ => TrySelectOrCombo(3);

            // Активация (ПКМ)
            input.actions["RightMouse"].started += _ => StartCasting();
            input.actions["RightMouse"].canceled += _ => StopCasting();
        }

        private void CycleSelection(float scrollValue)
        {
            if (_isCasting) return; // Нельзя менять основную способность во время каста
            
            if (scrollValue > 0) SelectAbility(_selectedIndex + 1);
            else if (scrollValue < 0) SelectAbility(_selectedIndex - 1);
        }

        private void SelectAbility(int index)
        {
            if (index < 0) index = _abilities.Count - 1;
            if (index >= _abilities.Count) index = 0;

            _selectedIndex = index;
            OnAbilitySelected?.Invoke(_selectedIndex);
        }

        private void TrySelectOrCombo(int index)
        {
            if (index < 0 || index >= _abilities.Count) return;

            if (_isCasting)
            {
                // Логика КОМБО: если уже кастуем, то нажатие другой цифры включает комбо
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
            
            // Проверка теплоты перед стартом (базовая)
            if (_globalData.Get<RuntimePlayerData>().CurrentWarmth <= 0) return;

            _activeAbility = _abilities[_selectedIndex];
            _activeAbility.Enabled = true;
            _activeAbility.ResetCombo(); // Сброс состояний
            _activeAbility.StartAbility?.Invoke();
            _isCasting = true;
        }

        private void ActivateCombo(IAbilityExtended secondary)
        {
            if (_comboAbility != null) return; // Уже есть комбо

            _comboAbility = secondary;
            // Уведомляем активную способность, что она теперь в режиме комбо с типом вторичной
            _activeAbility.SetComboContext(_comboAbility.AbilityType);
            
            OnComboUpdated?.Invoke(_activeAbility.AbilityType, _comboAbility.AbilityType);
        }

        private void StopCasting()
        {
            if (_activeAbility == null) return;

            _activeAbility.EndAbility?.Invoke();
            _activeAbility.ResetCombo();
            _activeAbility.Enabled = false;
            _activeAbility = null;
            _comboAbility = null;
            _isCasting = false;
            
            OnComboUpdated?.Invoke(null, null);
        }

        public void Tick()
        {
            // Глобальная проверка ресурсов
            if (_isCasting && _globalData.Get<RuntimePlayerData>().CurrentWarmth <= 0)
            {
                StopCasting();
            }
        }
    }
}
