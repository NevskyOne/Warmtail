using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class SettingsData : ISavableData
    {
        [Range(0.001f,1)] public float MainSoundVolume;
        [Range(0.001f,1)] public float MusicVolume;
        [Range(0.001f,1)] public float SfxVolume;
        public int QualityLevel;
        public bool FullscreenMode;
        public int Language;
    }
}