using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Systems.DataSystems
{
    public delegate void DataEventFunc();
    
    public class GlobalDataSystem : MonoBehaviour, IDisposable
    {
        [Title("Data")]
        [SerializeReference] private List<ISavableData> _savableData = new();
        [SerializeReference] private List<IRuntimeData> _runtimeData = new();
        
        [ShowInInspector, ReadOnly] private readonly Dictionary<IData, List<DataEventFunc>> _subs = new();

        private SaveSystem _saveSystem;

        [Inject]
        private void Construct(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _saveSystem.Load(ref _savableData);
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
                if(!_subs.ContainsKey(key))_subs[key] = new List<DataEventFunc>();
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
                _saveSystem.Save((ISavableData)foundKey);
            Debug.Log($"Updated {typeof(T).Name}");
        }
        
        public void Edit<T>(Action<T> mutator) where T : class, IData {
            var foundKey = _subs.Keys.First(data => typeof(T) == data.GetType());
            mutator((T)foundKey);
            NotifySubscribers<T>(); 
        }

        public T Get<T>() where T : class, IData
        {
            return (T)_subs.Keys.First(data => typeof(T) == data.GetType());
        }

        public void Dispose()
        {
            _subs.Clear();
        }
    }
}