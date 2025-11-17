using System.Collections.Generic;
using R3;
using TMPro;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Entities.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField, Dropdown(nameof(GetDropdownStrings))] private string _key;
        private TMP_Text Text => GetComponent<TMP_Text>();
        private LocalizationManager _localization;
    
        [Inject]
        private void Construct(LocalizationManager localization)
        {
            _localization = localization;
            _localization.CurrentLanguage.Subscribe(_ => UpdateString());
        }

        private void UpdateString()
        {
            print("UPD");
            Text.text = _localization.GetStringFromKey(_key);
        }
        
        public static IEnumerable<TriDropdownItem<string>> GetDropdownStrings()
        {
            TriDropdownList<string> list = new();
            foreach (var tableName in LocalizationManager.NameToGid.Keys)
            {
                var table = Resources.Load<TextAsset>($"Localization/{tableName}");
                string[] lines = table.text.Split('\n');
                for (int i = 1; i < lines.Length; i++)
                {
                    var key = lines[i].Split("\t")[0];
                    list.Add(new TriDropdownItem<string>{ 
                        Text = $"{tableName}/{key}", Value = key});
                }
            }

            return list;
        }
    }
}
