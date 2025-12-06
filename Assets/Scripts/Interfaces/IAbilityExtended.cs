using System;

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
