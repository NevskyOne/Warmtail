using System;
using Interfaces;
using UnityEngine;

namespace Systems.Abilities
{
    public abstract class BaseAbility : IAbilityExtended
    {
        public bool Enabled { get; set; }
        public Action StartAbility { get; set; }
        public Action UsingAbility { get; set; }
        public Action EndAbility { get; set; }
        [field: SerializeReference] public IAbilityVisual Visual { get; set; }
        
        public Type AbilityType => this.GetType();
        public bool IsComboActive { get; set; }
        protected Type _secondaryComboType;

        public void SetComboContext(Type secondaryAbilityType)
        {
            IsComboActive = true;
            _secondaryComboType = secondaryAbilityType;
            OnComboActivated();
        }

        public void ResetCombo()
        {
            IsComboActive = false;
            _secondaryComboType = null;
            OnComboReset();
        }

        protected virtual void OnComboActivated() { }
        protected virtual void OnComboReset() { }
    }
}