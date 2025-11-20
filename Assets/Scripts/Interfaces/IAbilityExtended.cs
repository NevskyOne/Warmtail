using System;

namespace Interfaces
{
    public interface IAbilityExtended
    {
        Type AbilityType { get; }
        bool IsComboActive { get; set; }
        void SetComboContext(Type secondaryAbilityType);
        void ResetCombo();
    }
}