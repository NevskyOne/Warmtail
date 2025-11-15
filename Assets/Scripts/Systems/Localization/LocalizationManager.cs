using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public Dictionary<string, string[]> _localizedText = new();
    private string[] headers;
    private const string TABLE_FILE_NAME = "LocalizatedTable.csv";
    private const string URL = "https://docs.google.com/spreadsheets/d/15TMb-C7uFx3gdVH0lnKPohJivmMyv1o4gv83MOUCdyo/export?format=csv&usp=sharing";
      
    [SerializeField] private string CurrentLanguage { get; set; }
    public static LocalizationManager Instance { get; private set; }
    
    void Awake()
    {
        CurrentLanguage = "ru_RU";
        LoadLocalizationTable();
    }
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
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        var table = Resources.Load<TextAsset>(TABLE_FILE_NAME.Replace(".csv", ""));
        if (!table)
        {
            Debug.LogError("No language table located! Make sure to load table!");
            return;
        }

        string[] lines = table.text.Split('\n');
        headers = lines[0].Split(',');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] translations = lines[i].Split(",");
            string keyId = translations[0];
            _localizedText[keyId] = translations;
        }
    }

    public string GetStringFromKey(string key)
    {
        if (_localizedText[key] == null) return "еще не загрузился язык";
        foreach(string s in headers) Debug.Log(s);
        Debug.Log(Array.IndexOf(headers, CurrentLanguage) + " " + CurrentLanguage);
        return _localizedText[key][Array.IndexOf(headers, CurrentLanguage)];
    }
}