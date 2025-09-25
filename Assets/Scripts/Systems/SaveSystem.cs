using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using R3;
using UnityEngine;

namespace Systems
{
    public enum SaveMode
    {
        Auto,
        Manual
    }
    
    public class SaveSystem : IDisposable
    {
        private readonly List<ISaveable> _saveables = new();
        private readonly CompositeDisposable _disposables = new(); 
        
        private SaveMode _saveMode = SaveMode.Auto;

        public SaveMode CurrentMode => _saveMode;  

        public void AddSaveable(ISaveable saveable)
        {
            if (_saveables.Contains(saveable)) return;

            _saveables.Add(saveable);
            
            if (_saveMode == SaveMode.Auto)
            {
                saveable.OnChanged
                    .ThrottleFirst(TimeSpan.FromSeconds(2))
                    .Subscribe(_ => Save(saveable))
                    .AddTo(_disposables);
            }
        }

        public void RemoveSaveable(ISaveable saveable)
        {
            _saveables.Remove(saveable);
        }
        
        public void SetSaveMode(SaveMode mode)
        {
            if (_saveMode == mode) return;

            _saveMode = mode;
            SaveSaveMode();
            
            if (mode == SaveMode.Manual)
            {
                _disposables.Clear();  
            }
            else
            {
                ReSubscribeToAuto();
            }
        }
        
        private void SaveSaveMode()
        {
            PlayerPrefs.SetInt("SaveMode", (int)_saveMode);
            PlayerPrefs.Save();
        }
        
        private void ReSubscribeToAuto()
        {
            _disposables.Clear();
            foreach (var saveable in _saveables)
            {
                saveable.OnChanged
                    .ThrottleFirst(TimeSpan.FromSeconds(2))
                    .Subscribe(_ => Save(saveable))
                    .AddTo(_disposables);
            }
        }
        
        public async UniTask LoadInit()
        {
            _saveMode = (SaveMode)PlayerPrefs.GetInt("SaveMode", (int)SaveMode.Auto);

            foreach (var saveable in _saveables)
            {
                var key = saveable.SaveKey;
                var json = PlayerPrefs.GetString(key, "{}");

                if (!string.IsNullOrEmpty(json) && json != "{}")
                {
                    saveable.FromJson(json);
                }
            }
            
            if (_saveMode == SaveMode.Auto)
            {
                ReSubscribeToAuto();
            }
        }
        
        public async UniTask SaveAll()
        {
            foreach (var saveable in _saveables)
            {
                await Save(saveable);
            }
        }
        
        private async UniTask Save(ISaveable saveable)
        {
            var key = saveable.SaveKey;
            var json = saveable.ToJson();
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            foreach (var saveable in _saveables)
            {
                saveable.Dispose();
            }
            _saveables.Clear();
        }
    }
}