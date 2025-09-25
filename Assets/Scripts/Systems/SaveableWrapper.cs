using Interfaces;
using R3;
using UnityEngine;

namespace Systems
{
    public class SaveableWrapper<T> : ISaveable where T : new()  
    {
        public string SaveKey { get; } 

        public ReactiveProperty<T> Data { get; } 

        public Observable<Unit> OnChanged => Data.Skip(1).Select(_ => Unit.Default);

        public SaveableWrapper(string saveKey, T initialData = default)
        {
            SaveKey = string.IsNullOrEmpty(saveKey) ? typeof(T).Name : saveKey;  
            Data = new ReactiveProperty<T>(initialData ?? new T());
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(Data.Value);
        }

        public void FromJson(string json)
        {
            Data.Value = JsonUtility.FromJson<T>(json) ?? new T(); 
        }

        public void Dispose()
        {
            Data?.Dispose();
        }
    }
}