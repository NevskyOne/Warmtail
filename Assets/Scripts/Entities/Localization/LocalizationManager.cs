using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Data;
using R3;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Entities.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private Dictionary<string, string[]> _localizedText = new();
        private string[] _headers;
        [SerializeField] private string _tableId = "1PEswgSetu71j068EhzqhuEPMUwGHdLbFa2sXZ9EeWAg";
        public static readonly Dictionary<string, string> NameToGid = new ()
        {
            {"UI", "1087436388"},
            {"Player", "1556233291"}
        };

        public ReactiveProperty<Language> CurrentLanguage { get; private set; } = new(Language.ru);

        [Inject] private GlobalData _globalData;
        
        private void Start()
        {
            SetValuesForTextsId();
            CurrentLanguage.Value = Language.ru;
            CurrentLanguage.ForceNotify();
        }

        [Button("Pull Table")]
        private void LoadLocalizationTable()
        {
            List<string> loaded = new();
            foreach (var tableName in NameToGid.Keys)
            {
                var req = UnityWebRequest.Get(
                    $"https://docs.google.com/spreadsheets/d/{_tableId}/export?format=tsv&gid={NameToGid[tableName]}");
                req.downloadHandler = new DownloadHandlerBuffer();
                var op = req.SendWebRequest();

                op.completed += _ =>
                {
                    if (loaded.Contains(tableName)) return;
                    loaded.Add(tableName);
                    var path = Path.Combine(Application.dataPath, "Resources/Localization", tableName);
                    if(File.Exists(path + ".tsv"))
                        File.Delete(path + ".tsv");
                    if(File.Exists(path + ".txt"))
                        File.Delete(path + ".txt");
                    var txt = req.downloadHandler.data;
                    using var tab = File.Create(path + ".tsv");
                    using var tabTxt = File.Create(path + ".txt");
                    tab.Write(txt);
                    tabTxt.Write(txt);
                    AssetDatabase.Refresh();
                    print("Loaded " + tableName);
                };
            }
        }
    
        private void SetValuesForTextsId()
        {
            foreach (var tableName in NameToGid.Keys)
            {
                var table = Resources.Load<TextAsset>($"Localization/{tableName}");
                if (!table)
                {
                    Debug.LogError("No language table located! Make sure to load table!");
                    return;
                }

                string[] lines = table.text.Split('\n');
                _headers = lines[0].Split("\t");
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] translations = lines[i].Split("\t");
                    string keyId = translations[0];
                    _localizedText[keyId] = translations;
                }
            }
        }
        
        public string GetStringFromKey(string key)
        {
            if (_localizedText[key] == null) return "Language haven`t loaded yet!";
            return ParseVars(_localizedText[key][Array.IndexOf(_headers, CurrentLanguage.ToString())]);
        }
        
        private string ParseVars(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            StringBuilder result = new StringBuilder(input.Length);
            StringBuilder key = new StringBuilder(32);
            bool inKey = false;
            
            foreach (char c in input)
            {
                if (c == '{')
                {
                    inKey = true;
                    key.Clear();
                    continue;
                }
                if (c == '}' && inKey)
                {
                    inKey = false;
                    if (_globalData.Get<DialogueVarData>().Variables.Exists(
                            x => x.Name == key.ToString()))
                    {
                        result.Append(_globalData.Get<DialogueVarData>().Variables.Find(
                            x => x.Name == key.ToString()).Value);
                    }
                    else result.Append('{').Append(key).Append('}');
                    continue;
                }
                if (inKey) key.Append(c);
                else result.Append(c);

            }
            return result.ToString();
        }
    }
    
    public enum Language
    {
        ru, en
    }
}