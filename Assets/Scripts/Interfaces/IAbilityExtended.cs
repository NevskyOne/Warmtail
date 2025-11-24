using System;using Interfaces;

namespace Interfaces
{
    public interface IAbilityExtended : IAbility
    {
        Type AbilityType { get; }
        bool IsComboActive { get; set; }
        void SetComboContext(Type secondaryAbilityType);
        void ResetCombo();
    }
}