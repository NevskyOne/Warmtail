using AYellowpaper.SerializedCollections;
using System;

namespace Data
{
    [Serializable]
    public class ShellsData : ISavableData
    {
        public SerializedDictionary<string, bool> ShellsActive; //[pos] = isActive
    }
}