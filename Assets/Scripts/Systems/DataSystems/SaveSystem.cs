using System.Collections.Generic;
using Data.Player;
using UnityEngine;


namespace Systems.DataSystems
{
    public class SaveSystem
    {
        public void Save(ISavableData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(data.GetType().Name, json);
            PlayerPrefs.Save();
        }

        public ISavableData Load(ISavableData data)
        {
            var json = PlayerPrefs.GetString(data.GetType().Name);
            return json != "" ? (ISavableData)JsonUtility.FromJson(json, data.GetType()) : data;
        }
        
        public void Load(ref List<ISavableData> dataList)
        {
            for (var i = 0; i < dataList.Count;  i++)
            {
                dataList[i] = Load(dataList[i]);
            }
        }
    }
}