using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Systems.DataSystems
{
    [Serializable]
    public class SaveContainer
    {
        public Dictionary<string, object> Blocks = new Dictionary<string, object>();
    }

    public class SaveSystem
    {
        private readonly string _fileName = "saves.json";
        private SaveContainer _container;

        public SaveContainer Container => _container;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        private string FilePath => Path.Combine(Application.persistentDataPath, _fileName);

        public void Load(ref List<ISavableData> dataList)
        {
            if (dataList == null) return;

            var diskContainer = LoadContainerFromDisk();
            var fileExists = diskContainer != null && diskContainer.Blocks != null && diskContainer.Blocks.Count > 0;

            if (!fileExists)
            {
                _container = new SaveContainer();
                foreach (var d in dataList)
                {
                    if (d == null) continue;
                    var key = d.GetType().Name;
                    _container.Blocks[key] = d;
                }
                WriteContainerToDisk(_container);
            }
            else
            {
                _container = diskContainer;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                var proto = dataList[i];
                if (proto == null) continue;
                var key = proto.GetType().Name;
                if (_container.Blocks.TryGetValue(key, out var stored))
                {
                    var intermediateJson = JsonConvert.SerializeObject(stored, _settings);
                    var loaded = (ISavableData)JsonConvert.DeserializeObject(intermediateJson, proto.GetType(), _settings);
                    if (loaded != null) dataList[i] = loaded;
                }
                else
                {
                    _container.Blocks[key] = proto;
                }
            }
        }

        public void UpdateData(ISavableData data)
        {
            if (data == null) return;
            if (_container == null) _container = new SaveContainer();
            var key = data.GetType().Name;
            _container.Blocks[key] = data;
        }

        public void SaveAllToDisk()
        {
            if (_container == null) return;
            WriteContainerToDisk(_container);
        }

        public ISavableData Load(ISavableData prototype)
        {
            if (prototype == null) return null;
            if (_container == null) _container = LoadContainerFromDisk() ?? new SaveContainer();
            var key = prototype.GetType().Name;
            if (!_container.Blocks.TryGetValue(key, out var stored)) return prototype;
            var json = JsonConvert.SerializeObject(stored, _settings);
            var loaded = (ISavableData)JsonConvert.DeserializeObject(json, prototype.GetType(), _settings);
            return loaded ?? prototype;
        }

        private SaveContainer LoadContainerFromDisk()
        {
            try
            {
                if (!File.Exists(FilePath)) return null;
                var txt = File.ReadAllText(FilePath);
                if (string.IsNullOrWhiteSpace(txt)) return null;
                var container = JsonConvert.DeserializeObject<SaveContainer>(txt, _settings);
                return container ?? null;
            }
            catch (Exception e)
            {
                Debug.LogError($"LoadContainerFromDisk error: {e.Message}");
                return null;
            }
        }

        private void WriteContainerToDisk(SaveContainer container)
        {
            try
            {
                var dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var json = JsonConvert.SerializeObject(container, _settings);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"WriteContainerToDisk error: {e.Message}");
            }
        }
    }
}
