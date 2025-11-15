using System;
using System.Collections.Generic;
using System.IO;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Entities.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private Dictionary<string, string[]> _localizedText = new();
        private string[] _headers;
        private const string TABLE_FILE_NAME = "LocalizatedTable.csv";
        private const string URL = "https://docs.google.com/spreadsheets/d/1PEswgSetu71j068EhzqhuEPMUwGHdLbFa2sXZ9EeWAg/export?format=csv&usp=sharing";
        [field: SerializeReference] public string CurrentLanguage { get; private set; }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        [Button("Pull Table")]
        private void LoadLocalizationTable()
        {
            var req = UnityWebRequest.Get(URL);
            req.downloadHandler = new DownloadHandlerBuffer();
            var op = req.SendWebRequest();

            op.completed += (_) =>
            {
                var txt = req.downloadHandler.data; 
                using var tab = File.Create(Path.Combine(Application.dataPath, "Resources", TABLE_FILE_NAME));
                tab.Write(txt);
                AssetDatabase.Refresh();
                SetValuesForTextsId();
            };
        }
    
        private void SetValuesForTextsId()
        {
            var table = Resources.Load<TextAsset>(TABLE_FILE_NAME.Replace(".csv", ""));
            if (!table)
            {
                Debug.LogError("No language table located! Make sure to load table!");
                return;
            }

            string[] lines = table.text.Split('\n');
            _headers = lines[0].Split(',');
            for (int i = 1; i < lines.Length; i++)
            {
                string[] translations = lines[i].Split(",");
                string keyId = translations[0];
                _localizedText[keyId] = translations;
            }
        }

        public string GetStringFromKey(string key)
        {
            if (_localizedText[key] == null) return "Language haven`t loaded yet!";
            foreach(string s in _headers) Debug.Log(s);
            Debug.Log(Array.IndexOf(_headers, CurrentLanguage) + " " + CurrentLanguage);
            return _localizedText[key][Array.IndexOf(_headers, CurrentLanguage)];
        }
    }
}