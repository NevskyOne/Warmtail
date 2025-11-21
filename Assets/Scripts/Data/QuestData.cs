using System;
using System.Collections.Generic;
using System.IO;
using Entities.Localization;
using TriInspector;
using UnityEngine;

namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "QuestData", menuName = "Configs/QuestData")]
    public class QuestData : ScriptableObject
    {
        [field: SerializeField, Dropdown(nameof(GetDropdownStrings))] public string Header { get; private set; }
        [field: SerializeField, Dropdown(nameof(GetDropdownStrings))] public string Description { get; private set; }
        [field: SerializeField] public int Layer { get; private set; }
        [field: SerializeField] public List<Vector2> Targets { get; private set; }
        
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