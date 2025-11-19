using System.Collections.Generic;
using Entities.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Entities.UI
{
    public class Switcher : MonoBehaviour
    {
        [SerializeField] private LocalizedText _localizedText;
        [SerializeField] private List<string> _id = new();
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _deActiveSprite;
        [SerializeField] private Transform _switchesParent;
        public UnityEvent<int> Event { get; set; } = new();
        public int CurrentValue { get; set; }
        private List<Image> _images = new();

        private void Start()
        {
            for (int i = 0; i < _switchesParent.childCount; i++)
            {
                _images.Add(_switchesParent.GetChild(i).GetComponent<Image>());
            }
            Switch(CurrentValue);
        }

        public void SwitchNext()
        {
            if (CurrentValue + 1 == _images.Count) return;
            Event.Invoke(CurrentValue + 1);
            Switch(CurrentValue + 1);
        }
        
        public void SwitchPrev()
        {
            if (CurrentValue == 0) return;
            Event.Invoke(CurrentValue - 1);
            Switch(CurrentValue - 1);
        }
        
        public virtual void Switch(int value)
        {
            _images[CurrentValue].sprite = _deActiveSprite;
            CurrentValue = value;
            _images[CurrentValue].sprite = _activeSprite;
            _localizedText.SetNewKey(_id[value]);
        }
    }
}