using System.Collections.Generic;
using UnityEngine;

namespace Entities.UI
{
    public class SectionsSwitcher : Switcher
    {
        [SerializeField] private List<GameObject> _sections;

        public override void Switch(int value)
        {
            _sections[CurrentValue].SetActive(false);
            base.Switch(value);
            _sections[value].SetActive(true);
        }
    }
}