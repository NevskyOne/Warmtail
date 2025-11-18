using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Systems.DataSystems;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Data
{
    public delegate void DataEventFunc();
    
    public class GlobalData : MonoBehaviour
    {
        [Title("Data")]
        [SerializeReference] private List<ISavableData> _savableData = new();

        public List<ISavableData> SavableData => _savableData;

        [SerializeReference] private List<IRuntimeData> _runtimeData = new();
        
        private readonly Dictionary<IData, List<DataEventFunc>> _subs = new();
        
        private SaveSystem _saveSystem;

        [Button("Delete Save Data"), GUIColor("red")]
        public void DeleteSaveData()
        {
            try
            {
                var filePath = Path.Combine(Application.persistentDataPath, "saves.json");
                var manualSavesPath = Path.Combine(Application.persistentDataPath, "manual_saves");
                if (File.Exists(filePath)) File.Delete(filePath);
                if(Directory.Exists(manualSavesPath)) Directory.Delete(manualSavesPath, true);
                Debug.Log("Data deleted!");
            }
            catch (Exception e)
            {
                Debug.LogError($"DeleteSaves error: {e.Message}");
            }
        }
        
        [Inject]
        private void Construct(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _saveSystem.Load(ref _savableData);

            var allDataList = _savableData.Concat<IData>(_runtimeData).ToList();
            foreach (var data in allDataList)
            {
                _subs.Add(data, new List<DataEventFunc>());
            }
        }
        
        public void SubscribeTo<T>(DataEventFunc selector) where T : class, IData
        {
            IData key = null;
            
            if (typeof(ISavableData).IsAssignableFrom(typeof(T)))
            {
                key = _savableData.Find(x => x.GetType() == typeof(T));
            }
            
            else if (typeof(IRuntimeData).IsAssignableFrom(typeof(T)))
            {
                key = _runtimeData.Find(x => x.GetType() == typeof(T));
            }
            
            if (key != null)
            {
                _subs[key].Add(selector);
                Debug.Log($"Added {typeof(T).Name}");
            }
            else Debug.Log($"No instance of type {typeof(T).Name} found in _savableData or _runtimeData.");
        }

        public void NotifySubscribers<T>() where T : class, IData
        {
            var foundKey = _subs.Keys.First(data => typeof(T) == data.GetType());

            foreach (var sub in _subs[foundKey])
            {
                sub.Invoke();
            }
            if (typeof(ISavableData).IsAssignableFrom(typeof(T)))
                _saveSystem.UpdateData((ISavableData)foundKey);
            Debug.Log($"Updated {typeof(T).Name}");
        }
        
        public void Edit<T>(Action<T> mutator) where T : class, IData {
            var foundKey = _subs.Keys.First(data => typeof(T) == data.GetType());
            mutator((T)foundKey);
            NotifySubscribers<T>(); 
        }

        public void UpdateAllData(List<ISavableData> newList)
        {
            if (newList == null) return;

            for (int i = 0; i < newList.Count; i++)
            {
                var newItem = newList[i];
                if (newItem == null) continue;
                
                IData oldKey = _subs.Keys.FirstOrDefault(k => k.GetType() == newItem.GetType());

                List<DataEventFunc> subscribers;
                if (oldKey != null && _subs.TryGetValue(oldKey, out subscribers))
                {
                    _subs.Remove(oldKey);
                }
                else
                {
                    subscribers = new List<DataEventFunc>();
                }
                
                _subs[newItem] = subscribers;

                int index = _savableData.FindIndex(x => x != null && x.GetType() == newItem.GetType());
                if (index >= 0)
                {
                    _savableData[index] = newItem;
                }
                else
                {
                    _savableData.Add(newItem);
                }
                
                foreach (var sub in subscribers)
                {
                    try { sub.Invoke(); } catch (Exception ex) { Debug.LogError($"Subscriber threw: {ex}"); }
                }
            }
        }

        public T Get<T>() where T : class, IData
        {
            return (T)_subs.Keys.First(data => typeof(T) == data.GetType());
        }

        public void OnDisable()
        {
            _subs.Clear();
            _saveSystem.SaveAllToDisk();
        }
    }
}