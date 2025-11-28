using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Systems.DataSystems
{
    [Serializable]
    public class ManualSaveMeta
    {
        public string Id;
        public string Name;
        public DateTime CreatedAt;
        public string JsonPath;
        public string ThumbnailPath;
    }

    public class ManualSaveSystem
    {
        private readonly string _savesFolder = Path.Combine(Application.persistentDataPath, "manual_saves");
        private readonly string _thumbFolder = Path.Combine(Application.persistentDataPath, "manual_saves", "thumbnails");
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
        private readonly int _thumbWidth = 320;
        private readonly int _thumbHeight = 180;

        public ManualSaveSystem()
        {
            if (!Directory.Exists(_savesFolder)) Directory.CreateDirectory(_savesFolder);
            if (!Directory.Exists(_thumbFolder)) Directory.CreateDirectory(_thumbFolder);
        }

        private string SlotFolder(string slotId) => Path.Combine(_savesFolder, slotId);
        private string SlotJsonPath(string slotId) => Path.Combine(SlotFolder(slotId), $"slot_{slotId}.json");
        private string SlotThumbPath(string slotId) => Path.Combine(_thumbFolder, $"thumb_{slotId}.png");
        private string MetaIndexPath => Path.Combine(_savesFolder, "meta_index.json");

        private void SaveMetaIndex(List<ManualSaveMeta> metas)
        {
            var json = JsonConvert.SerializeObject(metas, _settings);
            File.WriteAllText(MetaIndexPath, json);
        }

        private List<ManualSaveMeta> LoadMetaIndex()
        {
            try
            {
                if (!File.Exists(MetaIndexPath)) return new List<ManualSaveMeta>();
                var txt = File.ReadAllText(MetaIndexPath);
                var list = JsonConvert.DeserializeObject<List<ManualSaveMeta>>(txt, _settings);
                return list ?? new List<ManualSaveMeta>();
            }
            catch
            {
                return new List<ManualSaveMeta>();
            }
        }
        
        public async UniTask<ManualSaveMeta> CreateManualSaveAsync(string name, SaveContainer container, Camera cam = null, CancellationToken token = default)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            var slotId = Guid.NewGuid().ToString("N");
            var slotFolder = SlotFolder(slotId);
            Directory.CreateDirectory(slotFolder);

            var jsonPath = SlotJsonPath(slotId);
            var thumbPath = SlotThumbPath(slotId);
            
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(container, _settings));
            
            try
            {
                var png = cam == null ? await CaptureScreenPngAsync(_thumbWidth, _thumbHeight, token) : await CaptureCameraPngAsync(cam, _thumbWidth, _thumbHeight, token);
                File.WriteAllBytes(thumbPath, png);
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                Debug.LogWarning($"ManualSave: cannot capture thumbnail: {e.Message}");
            }

            var meta = new ManualSaveMeta
            {
                Id = slotId,
                Name = name,
                CreatedAt = DateTime.UtcNow,
                JsonPath = jsonPath,
                ThumbnailPath = File.Exists(thumbPath) ? thumbPath : null
            };

            var metas = LoadMetaIndex();
            metas.Add(meta);
            SaveMetaIndex(metas);

            return meta;
        }

        public List<ManualSaveMeta> ListManualSaves()
        {
            var metas = LoadMetaIndex();
            metas.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));
            return metas;
        }

        public bool DeleteManualSave(string slotId)
        {
            var metas = LoadMetaIndex();
            var meta = metas.Find(m => m.Id == slotId);
            if (meta == null) return false;

            try
            {
                if (File.Exists(meta.JsonPath)) File.Delete(meta.JsonPath);
                if (!string.IsNullOrEmpty(meta.ThumbnailPath) && File.Exists(meta.ThumbnailPath)) File.Delete(meta.ThumbnailPath);

                var folder = SlotFolder(slotId);
                if (Directory.Exists(folder)) Directory.Delete(folder, true);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"DeleteManualSave: {e.Message}");
            }

            metas.RemoveAll(m => m.Id == slotId);
            SaveMetaIndex(metas);
            return true;
        }
        
        public List<ISavableData> LoadManualSave(string slotId, List<ISavableData> dataList)
        {
            var metas = LoadMetaIndex();
            var meta = metas.Find(m => m.Id == slotId);

            try
            {
                var txt = File.ReadAllText(meta.JsonPath);
                var container = JsonConvert.DeserializeObject<SaveContainer>(txt, _settings) ?? new SaveContainer();

                for (int i = 0; i < dataList.Count; i++)
                {
                    var proto = dataList[i];
                    var key = proto.GetType().Name;
                    if (container.Blocks.TryGetValue(key, out var stored))
                    {
                        var json = JsonConvert.SerializeObject(stored, _settings);
                        var loaded = (ISavableData)JsonConvert.DeserializeObject(json, proto.GetType(), _settings);
                        if (loaded != null) dataList[i] = loaded;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"LoadManualSave failed: {e.Message}");
            }
            return dataList;
        }
        
        private async UniTask<byte[]> CaptureScreenPngAsync(int targetW, int targetH, CancellationToken token = default)
        {
            await UniTask.WaitForEndOfFrame(token);
            var w = Screen.width;
            var h = Screen.height;
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            tex.Apply();
            
            var scaled = ScaleTexture(tex, targetW, targetH);
            UnityEngine.Object.DestroyImmediate(tex);

            var png = scaled.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate(scaled);
            return png;
        }

        private async UniTask<byte[]> CaptureCameraPngAsync(Camera cam, int targetW, int targetH, CancellationToken token = default)
        {
            var rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
            cam.targetTexture = rt;
            cam.Render();
            await UniTask.Yield(token);

            RenderTexture.active = rt;
            var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(rt);

            var scaled = ScaleTexture(tex, targetW, targetH);
            UnityEngine.Object.DestroyImmediate(tex);

            var png = scaled.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate(scaled);
            return png;
        }

        private Texture2D ScaleTexture(Texture2D src, int width, int height)
        {
            var dst = new Texture2D(width, height, src.format, false);
            var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(src, rt);
            var prev = RenderTexture.active;
            RenderTexture.active = rt;
            dst.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            dst.Apply();
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);
            return dst;
        }
    }
}
