using System.Collections.Generic;
using System.IO;
using R3;
using TMPro;
using TriInspector;
using UnityEngine;

namespace Entities.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField, Dropdown(nameof(GetDropdownStrings))] private string _key;
        private TMP_Text _text;
        
        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            LocalizationManager.CurrentLanguage.Subscribe(_ => UpdateString());
        }

        [Button("UpdateString")]
        public void UpdateString()
        {
            _text.text = LocalizationManager.GetStringFromKey(_key);
        }

        public void SetNewKey(string key)
        {
            _key = key;
            UpdateString();
        }
        
        private IEnumerable<TriDropdownItem<string>> GetDropdownStrings()
        {
            TriDropdownList<string> list = new();
            foreach (var tableName in LocalizationManager.NameToGid.Keys)
            {
                var table = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Localization", $"{tableName}.tsv"));
                string[] lines = table.Split('\n');
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
